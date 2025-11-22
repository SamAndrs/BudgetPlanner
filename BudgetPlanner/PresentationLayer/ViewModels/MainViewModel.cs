using BudgetPlanner.PresentationLayer.Commands;
using System.Windows.Input;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;

        public ViewModelBase CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; RaisePropertyChanged(); }
        }

        // Commands
        public ICommand NavigateDashboardCommand { get; }
        public ICommand NavigateBudgetPostsCommand { get; }
        public ICommand NavigateCategoriesCommand { get; }
        public ICommand NavigatePrognosisCommand { get; }
        public ICommand NavigateSettingsCommand { get; }

        // ViewModels
        public DashboardViewVM DashboardVM { get; }
        public BudgetPostsViewVM BudgetPostsViewVM { get; }
        public CategoriesViewVM CategoriesViewVM { get; }
        public PrognosisViewVM PrognosisViewVM { get; set; }
        public SettingsViewVM SettingsViewVM { get; set; }


        public MainViewModel()
        {
            // Initialize VMs
            DashboardVM = new DashboardViewVM();
            BudgetPostsViewVM = new BudgetPostsViewVM();

            // Initialize Commands
            NavigateDashboardCommand = new DelegateCommand(_ => NavigateToDashboard());
            NavigateBudgetPostsCommand = new DelegateCommand(_ => CurrentView = BudgetPostsViewVM);
            NavigateCategoriesCommand = new DelegateCommand(_ => CurrentView = CategoriesViewVM);
            NavigatePrognosisCommand = new DelegateCommand(_ => CurrentView = PrognosisViewVM);
            NavigateSettingsCommand = new DelegateCommand(_ => CurrentView = SettingsViewVM);

            // StandardView
            CurrentView = DashboardVM;
        }

        private void NavigateToDashboard()
        {
            System.Diagnostics.Debug.WriteLine("Navigating to Dashboard View");
            CurrentView = DashboardVM;
        }

    }
}
