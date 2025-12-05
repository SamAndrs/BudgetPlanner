using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using BudgetPlanner.DomainLayer.Services;
using System.Collections.ObjectModel;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class DashboardViewVM : ViewModelBase
    {
        private readonly BudgetPostService _postService;
        private IEnumerable<BudgetPost> _allPosts;

        public string ViewTitle { get; private set; }
        public DateTime ThisMonth { get; private set; }
        public string CurrentMonth { get; private set; }
        
        public double TotalIncomeThisMonth { get; set; }
        public double TotalExpensesThisMonth { get; set; }
        

        // Day summary grid + list
        public ObservableCollection<DaySummaryVM> Days { get; set; } = new();

        // Chart
        public List<double> ExpenseValues { get; set; } = new();
        public List<string> ExpenseLabels { get; set; } = new();
        public ObservableCollection<DaySummaryVM> ActiveDays { get; private set; }


        // Constructor
        public DashboardViewVM(BudgetPostService budgetPostService)
        {
            _postService = budgetPostService;
            ViewTitle = "Månadsöversikt";

            ThisMonth = DateTime.Now;
            CurrentMonth = ThisMonth.ToString("MMMM");

            LoadDataForMont();
        }


        // Methods
        private void LoadDataForMont()
        {
            _allPosts = _postService.GetPostsForMonth(ThisMonth.Year, ThisMonth.Month);

            // Build days UI list
            Days = new ObservableCollection<DaySummaryVM>(
                _postService.BuildDaySummaries(_allPosts, ThisMonth));

            // Totals
            TotalIncomeThisMonth = _postService.SumAmountsByType(_allPosts, BudgetPostType.Income);
            TotalExpensesThisMonth = _postService.SumAmountsByType(_allPosts, BudgetPostType.Expense);

            // Chart + list (create list of active days)
            ActiveDays = new ObservableCollection<DaySummaryVM>(
                Days.Where(d => !d.IsPlaceholder && d.Posts != null && d.Posts.Any()));

            (ExpenseValues, ExpenseLabels) = _postService.GetExpenseCategoryChartData(_allPosts);
        }
    }
}
