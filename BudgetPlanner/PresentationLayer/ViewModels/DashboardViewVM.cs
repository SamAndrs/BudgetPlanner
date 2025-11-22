using System.Collections.ObjectModel;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class DashboardViewVM : ViewModelBase
    {

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<string> _recentItems;

        public ObservableCollection<string> RecentItems
        {
            get { return _recentItems; }
            set { _recentItems = value; RaisePropertyChanged(); }
        }


        public DashboardViewVM()
        {
            // Mockad testdata
            Title = "Dashboard - översikt";

            RecentItems = new ObservableCollection<string>
            {
                "Inkomst: Lön - 28 500kr",
                "Utgift: Mat - 3 200kr",
                "Utgift: Hyra - 8 500kr",
                "Inkomst: Bonus - 5 000kr",
                "Utgift: Transport - 1 200kr"
            };
        }
    }
}
