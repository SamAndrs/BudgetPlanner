using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using BudgetPlanner.PresentationLayer.Commands;
using System.Windows.Input;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class BudgetPostTestVM : ViewModelBase
    {
        private BudgetPost _testPost;

        public BudgetPost TestPost
        {
            get => _testPost; 
            set { _testPost = value; RaisePropertyChanged(); }
        }


        public BudgetPostTestVM()
        {
            // Mock data
            TestPost = new BudgetPost
            {
                Id = 1,
                Amount = 100.0,
                Category = new Category { Id = 1, Name = "Testkategori" },
                TransactionType = TransactionType.Expense,
                Reccuring = Recurring.Monthly,
                Date = DateTime.Now
            };

            IncreaseAmountCommand = new DelegateCommand(_ => IncreaseAmount());

        }

        public ICommand IncreaseAmountCommand { get; }

        private void IncreaseAmount()
        {
            TestPost.Amount += 10;
            RaisePropertyChanged(nameof(TestPost));
            RaisePropertyChanged(nameof(TestPost.Amount));
        }


    }
}
