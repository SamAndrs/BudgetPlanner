using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using System.Collections.ObjectModel;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class DashboardViewVM : ViewModelBase
    {
        public string ViewTitle { get; set; } = string.Empty;

        public double TotalIncomeThisMonth { get; set; }

        public double TotalExpensesThisMonth { get; set; }

        public double NetBalanceThisMonth => TotalIncomeThisMonth - TotalExpensesThisMonth;

        public string CurrentMonth { get; set; }

        // Month grid
        public ObservableCollection<DaySummaryVM> Days { get; set; } = new();

        // Month list
        public ObservableCollection<DaySummaryVM> ActiveDays { get; set; } = new();

        private readonly IEnumerable<BudgetPost> _allPosts;

        private DateTime _thisMonth;
        public DateTime ThisMonth
        {
            get { return _thisMonth; }
            set
            {
                if (SetProperty(ref _thisMonth, value))
                    LoadDays();
            }
        }

        // Chart
        public List<double> ExpenseValues { get; set; } = new();
        public List<string> ExpenseLabels { get; set; } = new();


        // Contructor
        public DashboardViewVM()
        {
            ViewTitle = "Månadsöversikt";

            _allPosts = Mockposts();
            LoadDays();

            ThisMonth = DateTime.Now;
            CurrentMonth = DateTime.Now.ToString("MMMM");

            TotalIncomeThisMonth = SumAmountsByType(_allPosts, BudgetPostType.Income);
            TotalExpensesThisMonth = SumAmountsByType(_allPosts, BudgetPostType.Expense);

            LoadExpenseCategoryChart();
        }


        // Methods
        private void LoadDays()
        {
            Days.Clear();

            // Total days in month
            var daysInMonth = DateTime.DaysInMonth(ThisMonth.Year, ThisMonth.Month);

            // Weekday for first day of month (0=Sunday, 1=Monday, ...)
            var firstDayOfMonth = new DateTime(ThisMonth.Year, ThisMonth.Month, 1);
            int dayOfWeekOffset = ((int)firstDayOfMonth.DayOfWeek + 6) % 7; // Adjusta so monday=0, sunday=6

            // Add placeholder-days before first day
            for (int i = 0; i < dayOfWeekOffset; i++)
            {
                Days.Add(new DaySummaryVM { IsPlaceholder = true });
            }

            // Add real days
            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(ThisMonth.Year, ThisMonth.Month, day);

                // Fetch posts for the day
                var postsForDay = _allPosts
                    .Where(p => p.Date.HasValue && p.Date.Value.Date == date.Date)
                    .ToList();


                // Summarize income and expense for the day
                var income = postsForDay
                    .Where(p => p.PostType == BudgetPostType.Income)
                    .Sum(p => p.Amount);

                var expense = postsForDay
                    .Where(p => p.PostType == BudgetPostType.Expense)
                    .Sum(p => p.Amount);

                Days.Add(new DaySummaryVM
                {
                    DayNumber = day,
                    Date = date,
                    TotalIncome = income,
                    TotalExpense = expense,
                    IsPlaceholder = false,
                    Posts = postsForDay
                });

                // Create list for active days
                ActiveDays = new ObservableCollection<DaySummaryVM>(
                    Days.Where(d => !d.IsPlaceholder && d.Posts != null && d.Posts.Any()));
            }
        }

        private IEnumerable<BudgetPost> Mockposts()
        {
            var Categories = new[]
            {
               // Expenses
                new Category { Id = 1, Name = "Alla" },
                new Category { Id = 2, Name = "Mat" },
                new Category { Id = 3, Name = "Transport" },
                new Category { Id = 4, Name = "Kläder" },
                new Category { Id = 5, Name = "Skatt" },
                new Category { Id = 6, Name = "Hem" },
                new Category { Id = 7, Name = "Hobby" },
                new Category { Id = 8, Name = "Barn" },
                new Category { Id = 9, Name = "TV" },
                new Category { Id = 10, Name = "SaaS" },
                new Category { Id = 11, Name = "Prenumerationer" },
                new Category { Id = 12, Name = "Husdjur" },
                new Category { Id = 13, Name = "Underhållning" },

                // Income
                new Category { Id = 14, Name = "Lön" },
                new Category { Id = 15, Name = "Bidrag" },
                new Category { Id = 16, Name = "Extrainkomst" },

                new Category { Id = 17, Name = "Okänd" }
            };

            var now = DateTime.Now;

            var posts = new List<BudgetPost>
            {
                new BudgetPost { Id = 1, Amount = 3200, Category = Categories[1], CategoryId= Categories[1].Id, Description = "Veckohandling", Date = new DateTime(now.Year, now.Month, 1), Recurring= Recurring.Weekly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 2, Amount = 8500, Category = Categories[5], CategoryId= Categories[5].Id, Description = "Månadshyra", Date = new DateTime(now.Year, now.Month, 25), Recurring= Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 3, Amount = 28500, Category = Categories[13], CategoryId= Categories[13].Id, Description = "Lön för Juni", Date = new DateTime(now.Year, now.Month, 22), Recurring= Recurring.Monthly, PostType = BudgetPostType.Income },
                new BudgetPost { Id = 4, Amount = 1200, Category = Categories[2], CategoryId= Categories[2].Id, Description = "Busskort", Date = new DateTime(now.Year, now.Month, 1), Recurring= Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 5, Amount = 500, Category = Categories[12], CategoryId= Categories[12].Id, Description = "Biobesök", Date = new DateTime(now.Year, now.Month, 15), Recurring= Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 6, Amount = 1200, Category = Categories[14], CategoryId= Categories[14].Id, Description = "Bonus", Date = new DateTime(now.Year, now.Month, 15), Recurring= Recurring.None, PostType = BudgetPostType.Income }

            };

            return posts;
        }

        private double SumAmountsByType(IEnumerable<BudgetPost> posts, BudgetPostType type)
        {
            var sumAmount = 0.0;
            foreach (var post in posts)
            {
                if (post.PostType == type)
                    sumAmount += post.Amount;
            }

            return sumAmount;
        }

        // Build the expense category chart
        private void LoadExpenseCategoryChart()
        {
            var expenses = _allPosts
                .Where(p => p.PostType == BudgetPostType.Expense);

            double total = expenses.Sum(p => p.Amount);

            var grouped = expenses
                .GroupBy(p => p.Category)
                .Select(g => new 
                { 
                    Category = g.Key.Name,
                    Amount = g.Sum(x => x.Amount)
                }).ToList();

            ExpenseValues.Clear();
            ExpenseLabels.Clear();
            
            foreach(var g in grouped)
            {
                ExpenseValues.Add(g.Amount);
                ExpenseLabels.Add(g.Category);
            }
        }
    }
}
