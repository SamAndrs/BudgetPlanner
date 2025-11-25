using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using BudgetPlanner.PresentationLayer.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class BudgetPostsViewVM : ViewModelBase
    {
        public string Title { get; set; }

        private string _searchText = "";
        private Category _selectedCategory;
        private BudgetPostType _selectedType;
        private DateTime? _fromDate;
        private DateTime? _toDate;

        #region --- Properties ---
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; 
                RaisePropertyChanged();
                ApplyFilters();
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
        #endregion

        #region -- Actions --
        
        public Action<AddEditBudgetPostVM>? RequestOpenDialog { get; set; }
        public Action? RequestCloseDialog { get; set; }
        #endregion


        // Alla poster (mockad data)
        public ObservableCollection<BudgetPost> AllPosts { get; set; }

        // Visa i UI efter filter
       public ObservableCollection<BudgetPost> FilteredPosts { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        // Commands
        public ICommand AddNewBudgetPostCommand { get; }
        public ICommand EditPostCommand { get; }
        public ICommand DeletePostCommand { get; }



        public BudgetPostsViewVM()
        {
            Title= "Budgetposter";

            // MOCKAD DATA
            Categories = new ObservableCollection<Category>
            {
                new Category { Id = 1, Name = "Mat" },
                new Category { Id = 2, Name = "Transport" },
                new Category { Id = 3, Name = "Lön" },
                new Category { Id = 4, Name = "Underhållning" },
                new Category { Id = 5, Name = "Boende" }
            };

            AllPosts = new ObservableCollection<BudgetPost>
            {
                new BudgetPost { Id = 1, Amount = 3200, Category = Categories[0], Description = "Veckohandling", Date = DateTime.UtcNow.AddDays(-2), PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 2, Amount = 8500, Category = Categories[4], Description = "Månadshyra", Date = DateTime.UtcNow.AddDays(-10), PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 3, Amount = 28500, Category = Categories[2], Description = "Lön för Juni", Date = DateTime.UtcNow.AddDays(-15), PostType = BudgetPostType.Income },
                new BudgetPost { Id = 4, Amount = 1200, Category = Categories[1], Description = "Busskort", Date = DateTime.UtcNow.AddDays(-5), PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 5, Amount = 5000, Category = Categories[3], Description = "Biobesök", Date = DateTime.UtcNow.AddDays(-7), PostType = BudgetPostType.Expense }
            };

            FilteredPosts = new ObservableCollection<BudgetPost>(AllPosts);

            AddNewBudgetPostCommand = new DelegateCommand(AddPost);
            EditPostCommand = new DelegateCommand(EditPost);
            DeletePostCommand = new DelegateCommand(DeletePost);

            // TODO: Ladda verklig data från service, här.
        }

        private void DeletePost(object? obj)
        {
            if (obj is BudgetPost post)
            {
                AllPosts.Remove(post);
                // TODO: Ta bort från datakälla via service
                ApplyFilters();
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
                }
                RequestCloseDialog?.Invoke();
            };

            RequestOpenDialog?.Invoke(vm);
        }


        private void ApplyFilters()
        {
            var result = AllPosts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
                result = result.Where(x => x.Category.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            if (SelectedCategory != null)
                result = result.Where(x => x.Category.Id == SelectedCategory.Id);

            if (SelectedType != null)
                result = result.Where(x => x.PostType == SelectedType);

            if (FromDate != null)
                result = result.Where(x => x.Date >= FromDate);

            if (ToDate != null)
                result = result.Where(x => x.Date <= ToDate);

            FilteredPosts.Clear();
            foreach (var item in result)
                FilteredPosts.Add(item);
        }
    }
}
