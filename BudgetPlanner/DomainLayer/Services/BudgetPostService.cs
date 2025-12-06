using BudgetPlanner.DataAccessLayer;
using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using BudgetPlanner.PresentationLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.DomainLayer.Services
{
    public class BudgetPostService
    {
        private readonly AppDbContext _db;

        public BudgetPostService(AppDbContext db)
        {
            _db = db;
        }

        public List<BudgetPost> GetAllPosts() => _db.BudgetPosts.Include(p => p.Category).ToList();


        public async Task GenerateRecurringPostsAsync()
        {
            // Generate recurring posts for coming months
            var posts = await _db.BudgetPosts
                .Where(p => p.Recurring != Recurring.None)
                .ToListAsync();

            var now = DateTime.Today;

            var limit = now.AddMonths(12);

            foreach(var post in posts)
            {
                if (!post.Date.HasValue)
                    continue;

                var nextDate = GetNextOccurence(post.Date, post.Recurring);

                // If post doesn't belong to a recurring group....
                if (string.IsNullOrEmpty(post.RecurringGroupID))
                    post.RecurringGroupID = Guid.NewGuid().ToString();

                // Skip if already set for future
                while (nextDate <= limit)
                {
                    bool exists = _db.BudgetPosts.Any(p =>
                       p.RecurringGroupID == post.RecurringGroupID &&
                        p.Date.Value.Date == nextDate.Date
                    );

                    if(!exists)
                    {
                        _db.BudgetPosts.Add(new BudgetPost
                        {
                            Amount = post.Amount,
                            CategoryId = post.CategoryId,
                            Description = post.Description,
                            Date = nextDate,
                            PostType = post.PostType,
                            Recurring = post.Recurring,
                            RecurringGroupID = post.RecurringGroupID
                        });
                    }

                    nextDate = GetNextOccurence(nextDate, post.Recurring);
                }
            }

            await _db.SaveChangesAsync();
        }

        private DateTime GetNextOccurence(DateTime? date, Recurring recurring)
        {
            if(date == null)
                throw new InvalidOperationException("Recurring posts must have a Date value.");

            var d = date.Value;

            return recurring switch
            {
                Recurring.Weekly => d.AddDays(7),
                Recurring.Monthly => d.AddMonths(1),
                Recurring.Yearly => d.AddYears(1),
                _ => d
            };
        }


        #region == DASHBOARD VIEW_MODEL =================================
        public List<BudgetPost> GetPostsForMonth(int year, int month)
        {

            return _db.BudgetPosts
                .Include(p => p.Category)
                .Where(p => p.Date.HasValue &&
                p.Date.Value.Year == year &&
                p.Date.Value.Month == month)
                .ToList();

        }

        public double SumAmountsByType(IEnumerable<BudgetPost> posts, BudgetPostType type)
        {
            return posts
                .Where(p => p.PostType == type)
                .Sum(p => p.Amount);
        }

        public List<DaySummaryVM> BuildDaySummaries(IEnumerable<BudgetPost> posts, DateTime month)
        {
            var days = new List<DaySummaryVM>();

            int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
            var firstDay = new DateTime(month.Year, month.Month, 1);
            int dayOffset = ((int)firstDay.DayOfWeek + 6) % 7; // Adjust so monday=0, sunday=6

            // Placeholder days
            for (int i = 0; i < dayOffset; i++)
                days.Add(new DaySummaryVM { IsPlaceholder = true });

            // Real days (summarise posts and income + expense amounts for each day)
            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(month.Year, month.Month, day);

                var postsForDay = posts
                    .Where(p => p.Date.Value.Date == date)
                    .ToList();

                var income = postsForDay
                    .Where(p => p.PostType == BudgetPostType.Income)
                    .Sum(p => p.Amount);

                var expense = postsForDay
                    .Where(p => p.PostType == BudgetPostType.Expense)
                    .Sum(p => p.Amount);

                days.Add(new DaySummaryVM
                {
                    DayNumber = day,
                    Date = date,
                    TotalIncome = income,
                    TotalExpense = expense,
                    Posts = postsForDay,
                    IsPlaceholder = false
                });
            }

            return days;
        }

        public (List<double> values, List<string> labels) GetExpenseCategoryChartData(IEnumerable<BudgetPost> posts)
        {
            var expenses = posts
                .Where(p => p.PostType == BudgetPostType.Expense)
                .ToList();

            var grouped = expenses
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key.Name,
                    Amount = g.Sum(p => p.Amount)
                })
                .ToList();

            var values = grouped.Select(g => g.Amount).ToList();
            var labels = grouped.Select(g => g.Category).ToList();

            return (values, labels);
        }
        #endregion=


        #region == BUDGETPOSTS VIEWMODEL =================================
        public IEnumerable<BudgetPost> FilterPosts(
            IEnumerable<BudgetPost> posts,
            string searchText = null,
            Category selectedCategory = null,
            BudgetPostType? selectedType = null,
            DateTime? fromDate = null,
            DateTime? ToDate = null)
        {
            var result = posts.AsEnumerable();

            if (!string.IsNullOrEmpty(searchText))
            {
                result = result.Where(r =>
                r.Category.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                r.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }


            // Retrieve posts from specific category
            if (selectedCategory != null && selectedCategory.Id != 1) // 1 = "Alla"
                result = result.Where(r => r.CategoryId == selectedCategory.Id);

            if (fromDate.HasValue)
                result = result.Where(r => r.Date >= fromDate.Value);

            if (ToDate.HasValue)
                result = result.Where(r => r.Date <= ToDate.Value);

            if (selectedType.HasValue)
                result = result.Where(r => r.PostType == selectedType.Value);

            return result;

        }

        public void AddPost(BudgetPost post)
        {
            if (post.Recurring != Recurring.None)
            {
                post.RecurringGroupID = Guid.NewGuid().ToString();
            }
            _db.BudgetPosts.Add(post);
            _db.SaveChanges();
        }

        public void UpdatePost(BudgetPost post)
        {
            _db.BudgetPosts.Update(post);
            _db.SaveChanges();
        }

        public void DeletePost(BudgetPost post)
        {
            _db.BudgetPosts.Remove(post);
            _db.SaveChanges();
        }

        #endregion


        #region == PROGNOSIS VIEWMODEL ===========================

        public List<RecurringBudgetPostTemplate> GetRecurringTemplates() => _db.RecurringPosts.ToList();

        #endregion
    }
}
