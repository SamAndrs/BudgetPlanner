using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using BudgetPlanner.DomainLayer.Services;
using BudgetPlanner.PresentationLayer.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class BudgetPostsViewVM : ViewModelBase
    {
        private CategoryService _categoryService;
        private readonly BudgetPostService _postService;

        public string ViewTitle { get; set; } = "Budgetposter";

        #region fields + properties

        private string _searchText = "";
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; RaisePropertyChanged(); ApplyFilters(); }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; RaisePropertyChanged(); ApplyFilters(); }
        }

        private BudgetPostFilterOption _selectedTypeOption;

        public BudgetPostFilterOption SelectedTypeOption
        {
            get { return _selectedTypeOption; }
            set { _selectedTypeOption = value; RaisePropertyChanged(); ApplyFilters(); }
        }


        private DateTime? _fromDate;
        public DateTime? FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; RaisePropertyChanged(); ApplyFilters(); }
        }

        private DateTime? _toDate;
        public DateTime? ToDate
        {
            get { return _toDate; }
            set { _toDate = value; RaisePropertyChanged(); ApplyFilters(); }
        }

        private double _totalIncome;
        public double TotalIncome
        {
            get { return _totalIncome; }
            set { _totalIncome = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(TotalDifference)); }
        }

        private double _totalExpense;
        public double TotalExpense
        {
            get { return _totalExpense; }
            set { _totalExpense = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(TotalDifference)); }
        }

        public double TotalDifference => TotalIncome - TotalExpense;

        #endregion


        // Collections
        private List<BudgetPost> _allPosts;

        public ObservableCollection<BudgetPost> FilteredPosts { get; set; }
        public ObservableCollection<Category> Categories { get; }
        public ObservableCollection<BudgetPostFilterOption> BPTypeFilterOptions { get; }

        // Chart
        public Dictionary<string, int> CategoryCounts =>
            //FilteredPosts
            _allPosts
                .GroupBy(propfull => propfull.Category.Name)
                .ToDictionary(g => g.Key, g => g.Count());

        // Actions
        public Action<AddEditBudgetPostVM>? RequestOpenDialog { get; set; }
        public Action? RequestCloseDialog { get; set; }

        // Commands
        public ICommand AddNewBudgetPostCommand { get; }
        public ICommand EditPostCommand { get; }
        public ICommand DeletePostCommand { get; }


        // Constructor
        public BudgetPostsViewVM(CategoryService categoryService, BudgetPostService budgetPostService)
        {
            _categoryService = categoryService;
            _postService = budgetPostService;

            // Load data
            Categories = new ObservableCollection<Category>(_categoryService.GetAllCategories());
            _allPosts = _postService.GetAllPosts();
            FilteredPosts = new ObservableCollection<BudgetPost>(_allPosts);

            // PostType filter options
            BPTypeFilterOptions = new ObservableCollection<BudgetPostFilterOption>
            {
                new BudgetPostFilterOption { DisplayName = "Alla", Type = null },
                new BudgetPostFilterOption { DisplayName = "Inkomst", Type = BudgetPostType.Income },
                new BudgetPostFilterOption { DisplayName = "Utgift", Type = BudgetPostType.Expense },
            };
            SelectedTypeOption = BPTypeFilterOptions.First();

            // Commands
            AddNewBudgetPostCommand = new DelegateCommand(AddPost);
            EditPostCommand = new DelegateCommand(EditPost);
            DeletePostCommand = new DelegateCommand(DeletePost);

            RecalculateTotals();
        }

        // Methods
        private void ApplyFilters()
        {
            var filtered = _postService.FilterPosts(
                _allPosts, SearchText, SelectedCategory, SelectedTypeOption?.Type, FromDate, ToDate);

            FilteredPosts.Clear();
            foreach (var post in filtered)
                FilteredPosts.Add(post);

            RecalculateTotals();
            RaisePropertyChanged(nameof(CategoryCounts));
        }

        private void RecalculateTotals()
        {
            TotalIncome = FilteredPosts
                .Where(p => p.PostType == BudgetPostType.Income)
                .Sum(p => p.Amount);

            TotalExpense = FilteredPosts
                .Where(p => p.PostType == BudgetPostType.Expense)
                .Sum(p => p.Amount);

            RaisePropertyChanged(nameof(TotalDifference));
        }

        private void DeletePost(object obj)
        {
            if (obj is not BudgetPost post) return;

            _postService.DeletePost(post);
            _allPosts.Remove(post);
            ApplyFilters();
        }

        private void EditPost(object obj)
        {
            if (obj is not BudgetPost post) return;

            var clone = new BudgetPost
            {
                Id = post.Id,
                Amount = post.Amount,
                Category = post.Category,
                CategoryId = post.CategoryId,
                Description = post.Description,
                Date = post.Date,
                PostType = post.PostType,
                Recurring = post.Recurring,
            };

            var vm = new AddEditBudgetPostVM(clone, Categories);

            vm.CloseRequested += (success, editedPost) =>
            {
                if (success)
                {
                    _postService.UpdatePost(post);

                    var index = _allPosts.IndexOf(post);
                    _allPosts[index] = editedPost;

                    ApplyFilters();
                    RequestCloseDialog?.Invoke();
                }
            };

            RequestOpenDialog?.Invoke(vm);
        }

        private void AddPost(object? obj)
        {
           var vm = new AddEditBudgetPostVM(
                new BudgetPost { Date = DateTime.Today }, Categories);

            vm.CloseRequested += (success, post) =>
            {
                if (success)
                {
                    _postService.AddPost(post);
                    _allPosts.Add(post);
                    ApplyFilters();
                    RequestCloseDialog?.Invoke();
                }
            };

            RequestOpenDialog?.Invoke(vm);
        }     

        // Helper class
        public class BudgetPostFilterOption
        {
            public string DisplayName { get; set; }
            public BudgetPostType? Type { get; set; }
        }
    }
}
