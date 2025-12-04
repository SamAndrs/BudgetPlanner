using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using BudgetPlanner.PresentationLayer.Commands;
using System.Collections.ObjectModel;
using System.Security.AccessControl;
using System.Windows.Input;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class BudgetPostsViewVM : ViewModelBase
    {
        public string ViewTitle { get; set; } = "Budgetposter";

        private string _searchText = "";
        private Category _selectedCategory;
        private BudgetPostType _selectedType;
        private Recurring _recurring;
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private BudgetPostFilterOption _selectedTypeOption;

        private double _totalIncome;
        private double _totalExpense;


        #region --- Properties (full) ---
        public string SearchText
        {
            get { return _searchText; }
            set 
            { 
                if(_searchText != value)
                {
                    _searchText = value;
                    RaisePropertyChanged();
                    ApplyFilters();
                }
            }
        }

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value;
                RaisePropertyChanged();
                ApplyFilters();
            }
        }

        public BudgetPostType SelectedType
        {
            get { return _selectedType; }
            set { _selectedType = value;
                RaisePropertyChanged();
                ApplyFilters();
            }
        }

        public Recurring Recurring
        {
            get { return _recurring; }
            set { _recurring = value; 
                RaisePropertyChanged();
                ApplyFilters();
            }
        }

        public DateTime? FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value;
                RaisePropertyChanged();
                ApplyFilters();
            }
        }

        public DateTime? ToDate
        {
            get { return _toDate; }
            set { _toDate = value;
                RaisePropertyChanged();
                ApplyFilters();
            }
        }

        public double TotalIncome
        {
            get { return _totalIncome; }
            set { _totalIncome = value; 
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TotalDifference));
            }
        }

        public double TotalExpense
        {
            get { return _totalExpense; }
            set { _totalExpense = value; 
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TotalDifference));
            }
        }

        public BudgetPostFilterOption SelectedTypeOption
        {
            get { return _selectedTypeOption; }
            set
            {
                _selectedTypeOption = value;
                RaisePropertyChanged();
                ApplyFilters();
            }
        }

        public double TotalDifference => TotalIncome - TotalExpense;

        #endregion

        // Collections
        public List<BudgetPost> AllPosts { get; set; }
        public ObservableCollection<BudgetPost> FilteredPosts { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        public ObservableCollection<BudgetPostFilterOption> BPTypeFilterOptions { get; }


        // Chart
        public Dictionary<string, int> CategoryCounts =>
                //FilteredPosts
                AllPosts
                 .GroupBy(p => p.Category.Name)
                 .ToDictionary(g => g.Key, g => g.Count());


        // Actions
        public Action<AddEditBudgetPostVM>? RequestOpenDialog { get; set; }
        public Action? RequestCloseDialog { get; set; }
        public Action? CategorySummaryChartRequested { get; set; }

        // Commands
        public ICommand AddNewBudgetPostCommand { get; }
        public ICommand EditPostCommand { get; }
        public ICommand DeletePostCommand { get; }


        // Constructor
        public BudgetPostsViewVM()
        {

            // MOCKAD DATA  // TODO: Ladda verklig data från service, här.
            Categories = LoadCategories();
            AllPosts = LoadData();

            // Set BudgetPostType filteringoptions (income/ expense/ all)
            BPTypeFilterOptions = SetPostTypeOptions();
            
            FilteredPosts = new ObservableCollection<BudgetPost>(AllPosts);

            AddNewBudgetPostCommand = new DelegateCommand(AddPost);
            EditPostCommand = new DelegateCommand(EditPost);
            DeletePostCommand = new DelegateCommand(DeletePost);
        }


        private List<BudgetPost>LoadData()
        {
            var now = DateTime.Now;

            var posts = new List<BudgetPost>
            {
                // ==== Nuvarande månad ====
                new BudgetPost { Id = 1, Amount = 3200, Category = Categories[1], CategoryId= Categories[1].Id, Description = "Veckohandling", Date = new DateTime(now.Year, now.Month, 1), Recurring= Recurring.Weekly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 2, Amount = 8500, Category = Categories[5], CategoryId= Categories[5].Id, Description = "Månadshyra", Date = new DateTime(now.Year, now.Month, 25), Recurring= Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 3, Amount = 28500, Category = Categories[13], CategoryId= Categories[13].Id, Description = "Lön för Juni", Date = new DateTime(now.Year, now.Month, 22), Recurring= Recurring.Monthly, PostType = BudgetPostType.Income },
                new BudgetPost { Id = 4, Amount = 1200, Category = Categories[2], CategoryId= Categories[2].Id, Description = "Busskort", Date = new DateTime(now.Year, now.Month, 1), Recurring= Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 5, Amount = 500, Category = Categories[12], CategoryId= Categories[12].Id, Description = "Biobesök", Date = new DateTime(now.Year, now.Month, 15), Recurring= Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 6, Amount = 1200, Category = Categories[14], CategoryId= Categories[14].Id, Description = "Bonus", Date = new DateTime(now.Year, now.Month, 15), Recurring= Recurring.None, PostType = BudgetPostType.Income },
            
                // ===== Månad 2: En månad bakåt  =====
                new BudgetPost { Id = 1, Amount = 3200, Category = Categories[0], CategoryId = 2, Description = "Veckohandling", Date = new DateTime(now.Year, now.Month-1, 1), Recurring = Recurring.Weekly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 2, Amount = 900, Category = Categories[1], CategoryId = 3, Description = "Månadskort buss", Date = new DateTime(now.Year, now.Month-1, 2), Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 3, Amount = 8500, Category = Categories[4], CategoryId = 6, Description = "Hyra", Date = new DateTime(now.Year, now.Month-1, 25), Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 4, Amount = 1200, Category = Categories[10], CategoryId = 12, Description = "Veterinärkontroll", Date = new DateTime(now.Year, now.Month-1, 12), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 5, Amount = 28500, Category = Categories[12], CategoryId = 14, Description = "Lön", Date = new DateTime(now.Year, now.Month-1, 23), Recurring = Recurring.Monthly, PostType = BudgetPostType.Income },
                new BudgetPost { Id = 6, Amount = 800, Category = Categories[14], CategoryId = 16, Description = "Frilansjobb", Date = new DateTime(now.Year, now.Month-1, 18), Recurring = Recurring.None, PostType = BudgetPostType.Income },

                // ===== Månad 3: Två månader bakåt =====
                new BudgetPost { Id = 7, Amount = 3100, Category = Categories[0], CategoryId = 2, Description = "Storhandling", Date = new DateTime(now.Year, now.Month - 2, 3), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 8, Amount = 750, Category = Categories[1], CategoryId = 3, Description = "Resor till arbete", Date = new DateTime(now.Year, now.Month - 2, 6), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 9, Amount = 8500, Category = Categories[4], CategoryId = 6, Description = "Hyra", Date = new DateTime(now.Year, now.Month - 2, 25), Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 10, Amount = 200, Category = Categories[11], CategoryId = 13, Description = "Biobesök", Date = new DateTime(now.Year, now.Month - 2, 14), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 11, Amount = 28500, Category = Categories[12], CategoryId = 14, Description = "Lön", Date = new DateTime(now.Year, now.Month - 2, 23), Recurring = Recurring.Monthly, PostType = BudgetPostType.Income },
                new BudgetPost { Id = 12, Amount = 1500, Category = Categories[15], CategoryId = 16, Description = "Extrajobb helg", Date = new DateTime(now.Year, now.Month - 2, 20), Recurring = Recurring.None, PostType = BudgetPostType.Income },

                // ===== Månad 4: Tre månader bakåt =====
                new BudgetPost { Id = 13, Amount = 2800, Category = Categories[0], CategoryId = 2, Description = "Matinköp", Date = new DateTime(now.Year, now.Month - 3, 4), Recurring = Recurring.Weekly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 14, Amount = 8500, Category = Categories[4], CategoryId = 6, Description = "Hyra", Date = new DateTime(now.Year, now.Month - 3, 25), Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 15, Amount = 1200, Category = Categories[9], CategoryId = 11, Description = "Netflix + Spotify", Date = new DateTime(now.Year, now.Month - 3, 1), Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 16, Amount = 400, Category = Categories[2], CategoryId = 4, Description = "Nya strumpor", Date = new DateTime(now.Year, now.Month - 3, 10), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 17, Amount = 28500, Category = Categories[12], CategoryId = 14, Description = "Lön", Date = new DateTime(now.Year, now.Month - 3, 23), Recurring = Recurring.Monthly, PostType = BudgetPostType.Income },
                new BudgetPost { Id = 18, Amount = 900, Category = Categories[13], CategoryId = 15, Description = "Studiebidrag", Date = new DateTime(now.Year, now.Month - 3, 5), Recurring = Recurring.Monthly, PostType = BudgetPostType.Income },

                // ===== Månad 5: Fyra månader bakåt =====
                new BudgetPost { Id = 19, Amount = 3000, Category = Categories[0], CategoryId = 2, Description = "Matkasse", Date = new DateTime(now.Year, now.Month - 4, 6), Recurring = Recurring.Weekly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 20, Amount = 600, Category = Categories[1], CategoryId = 3, Description = "Bussbiljetter", Date = new DateTime(now.Year, now.Month - 4, 12), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 21, Amount = 8500, Category = Categories[4], CategoryId = 6, Description = "Hyra", Date = new DateTime(now.Year, now.Month - 4, 25), Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 22, Amount = 300, Category = Categories[6], CategoryId = 8, Description = "Barnaktiviteter", Date = new DateTime(now.Year, now.Month - 4, 8), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 23, Amount = 28500, Category = Categories[12], CategoryId = 14, Description = "Lön", Date = new DateTime(now.Year, now.Month - 4, 23), Recurring = Recurring.Monthly, PostType = BudgetPostType.Income },
                new BudgetPost { Id = 24, Amount = 600, Category = Categories[14], CategoryId = 16, Description = "Säljtjänst", Date = new DateTime(now.Year, now.Month - 4, 17), Recurring = Recurring.None, PostType = BudgetPostType.Income },
            };

            return posts;
        }

        private ObservableCollection<Category> LoadCategories()
        {
            return new ObservableCollection<Category>
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
        }

        private ObservableCollection<BudgetPostFilterOption> SetPostTypeOptions()
        {
            return new ObservableCollection<BudgetPostFilterOption>
            {
                new BudgetPostFilterOption { DisplayName = "Alla", Type = null },
                new BudgetPostFilterOption { DisplayName = "Inkomst", Type = BudgetPostType.Income},
                new BudgetPostFilterOption { DisplayName = "Utgift", Type = BudgetPostType.Expense}
            };
        }

        private void DeletePost(object? obj)
        {
            if (obj is BudgetPost post)
            {
                AllPosts.Remove(post);
                // TODO: Ta bort från datakälla via service
                ApplyFilters();
                RecalculateTotals();
                RaisePropertyChanged(nameof(CategoryCounts));
            }
        }

        private void EditPost(object? obj)
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
                Recurring = post.Recurring
            };

            var vm = new AddEditBudgetPostVM(clone, Categories);

            vm.CloseRequested += (success, editedPost) =>
            {
                if (success)
                {
                    // Uppdatera den ursprungliga posten med de redigerade värdena
                    post.Amount = editedPost.Amount;
                    post.Category = editedPost.Category;
                    post.CategoryId = editedPost.CategoryId;
                    post.Description = editedPost.Description;
                    post.Date = editedPost.Date;
                    post.PostType = editedPost.PostType;
                    post.Recurring = editedPost.Recurring;

                    ApplyFilters();
                    RecalculateTotals();
                    RaisePropertyChanged(nameof(CategoryCounts));
                }
                RequestCloseDialog?.Invoke();

            };

            RequestOpenDialog?.Invoke(vm);
        }

        private void AddPost(object? obj)
        {
            var vm = new AddEditBudgetPostVM(new BudgetPost { Date= DateTime.Today }, Categories);

            vm.CloseRequested += (success, post) =>
            {
                if (success)
                {
                    AllPosts.Add(post);
                    ApplyFilters();
                    RecalculateTotals();
                    RaisePropertyChanged(nameof(CategoryCounts));
                }
                RequestCloseDialog?.Invoke();
            };

            RequestOpenDialog?.Invoke(vm);

        }


        private void ApplyFilters()
        {
            var result = AllPosts.AsEnumerable();

            // Söktext
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                result = result.Where(x => 
                    x.Category.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    x.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }
            
            // Kategori: Hoppa över om 'Alla' är vald eller null
            if (SelectedCategory != null && SelectedCategory.Id != 1)
                result = result.Where(x => x.Category.Id == SelectedCategory.Id);

            // Datumintervall
            if (FromDate != null)
                result = result.Where(x => x.Date >= FromDate);

            if (ToDate != null)
                result = result.Where(x => x.Date <= ToDate);

            // Inkomst/Utgift
            if (SelectedTypeOption?.Type != null)
                result = result.Where(x => x.PostType == SelectedTypeOption.Type.Value);

            FilteredPosts.Clear();
            foreach (var item in result)
                FilteredPosts.Add(item);

            RaisePropertyChanged(nameof(TotalIncome));
            RaisePropertyChanged(nameof(TotalExpense));
            RecalculateTotals();
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

        // Helper class
        public class BudgetPostFilterOption
        {
            public string DisplayName { get; set; }
            public BudgetPostType? Type { get; set; }
        }
    }
}
