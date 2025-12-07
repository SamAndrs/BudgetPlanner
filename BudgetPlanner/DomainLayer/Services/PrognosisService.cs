using BudgetPlanner.DataAccessLayer;
using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.DomainLayer.Services
{
    public class PrognosisService
    {
        private readonly AppDbContext _context;

        public PrognosisService(AppDbContext context)
        {
            _context = context;
        }

        #region == PROGNOSIS VIEWMODEL ===========================
        public List<Prognosis> GetExistingPrognoses()
        {
            return _context.Prognoses
                .Include(p => p.BudgetPosts)
                .ThenInclude(bp => bp.Category)
                .OrderBy(p => p.FromDate)
                .ToList();
        }


        public Prognosis GeneratePrognosisForMonth(int year, int month)
        {
            // 1. Abort if prognosis for month already exist
            var exiting = _context.Prognoses
                .Include(p => p.BudgetPosts)
                .FirstOrDefault(p => p.FromDate.Year == year && p.FromDate.Month == month);

            if (exiting != null) 
                return exiting;

            // 2. If no existing prognosis, create new
            var prognosis = new Prognosis
            {
                Id = Guid.NewGuid(),
                FromDate = new DateTime(year, month, 1),
                ToDate = new DateTime(year, month, 1).AddMonths(1).AddDays(-1),
                Month = new DateTime(year, month, 1).ToString("MMMM yyyy")
            };

            // 3. Generate and add reccuring posts to Prognosis, from templates
            var recurring = GenerateRecurringPosts(year, month);

            prognosis.BudgetPosts = recurring.ToList();

            // 4. Summarize BudgetPost income/ expense values and add to Prognosis
            prognosis.MonthlyIncome = prognosis.BudgetPosts
                                                .Where(p => p.PostType == BudgetPostType.Income)
                                                .Sum(p => (decimal)p.Amount);
            prognosis.MonthlyExpense = prognosis.BudgetPosts
                                                .Where(p => p.PostType == BudgetPostType.Expense)
                                                .Sum(p => (decimal)p.Amount);
            prognosis.TotalSum = prognosis.MonthlyIncome - prognosis.MonthlyExpense;


            _context.Prognoses.Add(prognosis);
            _context.SaveChanges();

            return prognosis;
        }

        // Generate prognosis for next month based on recurring posts data.
        public Prognosis GetOrCreateNextMonthPrognosis()
        {
            var nextMonthDate = DateTime.Now.AddMonths(1);
            var from = new DateTime(nextMonthDate.Year, nextMonthDate.Month, 1);
            var to = from.AddMonths(1).AddDays(-1);

            // 1. If prognosis already exist, return it
            var existing = _context.Prognoses
                                     .Include(p => p.BudgetPosts)
                                     .ThenInclude(bp => bp.Category)
                                     .FirstOrDefault(p => p.FromDate.Year == from.Year && p.FromDate.Month == from.Month);

            if (existing != null)
                return existing;

            // 2. If no prognosis already exist, create new
            var newPrognosis = new Prognosis
            {
                Id = Guid.NewGuid(),
                FromDate = from,
                ToDate = to,
                Month = from.ToString("MMMM yyyy"),
                BudgetPosts = new List<BudgetPost>()
            };

            // 3. Add recurring BudgetPosts
            var recurring = GenerateRecurringPosts(from.Year, from.Month);

            newPrognosis.BudgetPosts = recurring.ToList();

            // 4. Summarize income/ expenses
            newPrognosis.MonthlyIncome = newPrognosis.BudgetPosts
                                               .Where(p => p.PostType == BudgetPostType.Income)
                                               .Sum(p => (decimal)p.Amount);
            newPrognosis.MonthlyExpense = newPrognosis.BudgetPosts
                                                .Where(p => p.PostType == BudgetPostType.Expense)
                                                .Sum(p => (decimal)p.Amount);
            newPrognosis.TotalSum = newPrognosis.MonthlyIncome - newPrognosis.MonthlyExpense;

            _context.Prognoses.Add(newPrognosis);
            _context.SaveChanges();

            return newPrognosis;
        }

        private List<BudgetPost> GenerateRecurringPosts(int year, int month)
        {
            // 1. Fetch list of stopped recurring-IDs
            var stoppedPosts = _context.StoppedRecurringPosts
                .Select(p => p.RecurringId)
                .ToHashSet();

            // 2. Fetch all currently NOT stopped templates
            var templates = _context.RecurringPosts
                .Where(t => !stoppedPosts.Contains(t.RecurringId))
                .ToList();

            var list = new List<BudgetPost>();

            // Loop through list of recurring BudgetPost templates,
            // and base new Prognosis Budgetpost data, depending on recurrence
            foreach (var template in templates)
            {
                var current = new DateTime(year, month, template.RecurringStartDate.Day);

                Action<DateTime> addPost = date =>
                {
                    list.Add(new BudgetPost
                    {
                        Amount = template.Amount,
                        CategoryId = template.CategoryId,
                        Recurring = template.Recurring,
                        Date = date,
                        PostType = template.PostType,
                        RecurringId = template.RecurringId
                    });
                };

                switch (template.Recurring)
                {
                    case Recurring.Monthly:
                        addPost(new DateTime(year, month, template.RecurringStartDate.Day));
                        break;

                    case Recurring.Weekly:

                        while (current.Month == month)
                        {
                            addPost(current);
                            current = current.AddDays(7);
                        }
                        break;

                    case Recurring.Daily:

                        while (current.Month == month)
                        {
                            addPost(current);
                            current = current.AddDays(1);
                        }
                        break;

                    case Recurring.Yearly:
                        if (template.RecurringStartDate.Month == month)
                        {
                            addPost(new DateTime(year, month, template.RecurringStartDate.Day));
                        }
                        break;
                }
            }

            return list;
        }      
        #endregion
    }
}
