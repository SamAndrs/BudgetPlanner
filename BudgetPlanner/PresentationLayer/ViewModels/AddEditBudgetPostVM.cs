using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using BudgetPlanner.PresentationLayer.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class AddEditBudgetPostVM : ViewModelBase
    {
        private BudgetPost _model;

        public string DialogTitle { get; set; }

        // Lists for ComboBoxes
        public ObservableCollection<Category> Categories { get; }
        public Array BudgetPostTypes => Enum.GetValues(typeof(BudgetPostType));
        public Array RecurringOptions => Enum.GetValues(typeof(Recurring));

        // Properties
        public double Amount
        {
            get { return _model.Amount; }
            set { _model.Amount = value; RaisePropertyChanged(); }
        }

        public Category Category
        {
            get { return _model.Category; }
            set { _model.Category = value; RaisePropertyChanged(); }
        }

        public BudgetPostType PostType
        {
            get { return _model.PostType; }
            set { _model.PostType = value; RaisePropertyChanged(); }
        }

        public Recurring Recurring
        {
            get { return _model.Recurring; }
            set { _model.Recurring = value; RaisePropertyChanged(); }
        }

        public DateTime Date
        {
            get { return (DateTime)_model.Date; }
            set { _model.Date = value; RaisePropertyChanged(); }
        }

        public string Description 
        { 
            get { return _model.Description;  } 
            set { _model.Description = value; RaisePropertyChanged(); }
        }

        // Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // Event to close the dialog or navigate back
        public event Action<bool, BudgetPost>? CloseRequested;


        public AddEditBudgetPostVM(BudgetPost model, ObservableCollection<Category> categories)
        {
            _model = model;
            Categories = categories;

            SaveCommand = new DelegateCommand(_ => Save());
            CancelCommand = new DelegateCommand(_ => Cancel());

            DialogTitle = "Redigera/ Addera post";
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(false, _model);
        }

        private void Save()
        {
            CloseRequested?.Invoke(true, _model);
        }
    }
}