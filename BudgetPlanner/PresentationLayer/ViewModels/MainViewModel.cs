using BudgetPlanner.DomainLayer.Services;
using BudgetPlanner.PresentationLayer.Commands;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;

        private bool _isDialogOpen;

        public bool IsDialogOpen
        {
            get { return _isDialogOpen; }
            set { _isDialogOpen = value; RaisePropertyChanged(); }
        }

        private object _currentDialog;

        public object CurrentDialog
        {
            get { return _currentDialog; }
            set { _currentDialog = value; RaisePropertyChanged(); }
        }


        public ViewModelBase CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; RaisePropertyChanged(); }
        }

        // Commands
        public ICommand NavigateDashboardCommand { get; }
        public ICommand NavigateBudgetPostsCommand { get; }
        public ICommand NavigatePrognosisCommand { get; }
        public ICommand NavigateSettingsCommand { get; }

        // ViewModels
        public DashboardViewVM DashboardVM { get; }
        public BudgetPostsViewVM BudgetPostsViewVM { get; }
        public PrognosisViewVM PrognosisViewVM { get; set; }
        public SettingsViewVM SettingsViewVM { get; set; }


        public MainViewModel(DashboardViewVM dashboardVM, BudgetPostsViewVM budgetPostsVM, PrognosisViewVM prognosisVM, SettingsViewVM settingsVM)
        {
            DashboardVM = dashboardVM;
            BudgetPostsViewVM = budgetPostsVM;
            PrognosisViewVM = prognosisVM;
            SettingsViewVM = settingsVM;


            // Initialize Commands
            NavigateDashboardCommand = new DelegateCommand(_ => NavigateToDashboard());
            NavigateBudgetPostsCommand = new DelegateCommand(_ => CurrentView = BudgetPostsViewVM);
            NavigatePrognosisCommand = new DelegateCommand(_ => CurrentView = PrognosisViewVM);
            NavigateSettingsCommand = new DelegateCommand(_ => CurrentView = SettingsViewVM);

            // koppla VM -> dialoghantering
            BudgetPostsViewVM.RequestOpenDialog = vm => OpenDialog(vm);
            BudgetPostsViewVM.RequestCloseDialog = () => CloseDialog();

            // StandardView
            CurrentView = DashboardVM;
        }

        private void NavigateToDashboard()
        {
            CurrentView = DashboardVM;
        }

        public void OpenDialog(object dialogVm)
        {
           CurrentDialog = dialogVm;
              IsDialogOpen = true;
        }

        public void CloseDialog()
        {
            IsDialogOpen = false;

            // Delay removal so binding cycle hinner uppdateras
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                CurrentDialog = null;
            }, DispatcherPriority.Background);
        }
    }
}
