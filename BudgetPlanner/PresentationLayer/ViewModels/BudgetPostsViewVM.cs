using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using BudgetPlanner.PresentationLayer.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class BudgetPostsViewVM : ViewModelBase
    {
        // Alla poster (mockad data)
        public ObservableCollection<BudgetPost> AllPosts { get; set; }

        // Visa i UI efter filter
       public ObservableCollection<BudgetPost> FilteredPosts { get; set; }

        public string SearchText { get; set; }
        public string SelectedCategory { get; set; }
        public BudgetPostType? SelectedType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        // Commands
        public ICommand AddNewBudgetPostCommand { get; }
        public ICommand EditPostCommand { get; }
        public ICommand DeletePostCommand { get; }

        public BudgetPostsViewVM()
        {
            // MOCKAD DATA
            Categories = new ObservableCollection<Category>
            {
                new Category { Id = 1, Name = "Mat" },
                new Category { Id = 2, Name = "Transport" },
                new Category { Id = 3, Name = "Lön" },
                new Category { Id = 4, Name = "Underhållning" }
            };

            AllPosts = new ObservableCollection<BudgetPost>
            {
                new BudgetPost { Id = 1, Amount = 3200, Category = Categories[0], Description = "Veckohandling", Date = DateTime.UtcNow.AddDays(-2), PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 2, Amount = 8500, Category = Categories[1], Description = "Månadshyra", Date = DateTime.UtcNow.AddDays(-10), PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 3, Amount = 28500, Category = Categories[2], Description = "Lön för Juni", Date = DateTime.UtcNow.AddDays(-15), PostType = BudgetPostType.Income },
                new BudgetPost { Id = 4, Amount = 1200, Category = Categories[1], Description = "Busskort", Date = DateTime.UtcNow.AddDays(-5), PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 5, Amount = 5000, Category = Categories[3], Description = "Biobesök", Date = DateTime.UtcNow.AddDays(-7), PostType = BudgetPostType.Expense }
            };

            FilteredPosts = new ObservableCollection<BudgetPost>(AllPosts);

            AddNewBudgetPostCommand = new DelegateCommand(AddPost);
            EditPostCommand = new DelegateCommand(EditPost);
            DeletePostCommand = new DelegateCommand(DeletePost);
        }

        private void DeletePost(object? obj)
        {
            // bara mock för testning
            AllPosts.Add(new BudgetPost
            {
                Date = DateTime.Today,
                PostType = BudgetPostType.Expense,
                Category = Categories[0],
                Amount = 150
            });
            ApplyFilters();
        }

        private void EditPost(object? obj)
        {
            // mock
            System.Diagnostics.Debug.WriteLine("Edit: " + obj);
        }

        private void AddPost(object? obj)
        {
            if (obj is BudgetPost post)
            {
                AllPosts.Remove(post);
                ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            var result = AllPosts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
                result = result.Where(x => x.Category.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            if (SelectedCategory != null)
                result = result.Where(x => x.Category.Name == SelectedCategory);

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
