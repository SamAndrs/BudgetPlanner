using BudgetPlanner.PresentationLayer.ViewModels;

namespace BudgetPlanner.DomainLayer.Services
{
    /*
        Service for handling global property values
     
     */

    public class UserSettingsService : ViewModelBase
    {
        private double _yearlyIncome;
        public double YearlyIncome
        {
            get { return _yearlyIncome; }
            set { _yearlyIncome = value; RaisePropertyChanged(); }
        }

        private double _yearlyWorkhours;
        public double YearlyWorkhours
        {
            get { return _yearlyWorkhours; }
            set { _yearlyWorkhours = value; RaisePropertyChanged(); }
        }


        //private double _weeklyWorkhours;
        //public double WeeklyWorkhours
        //{
        //    get { return _weeklyWorkhours; }
        //    set { _weeklyWorkhours = value; RaisePropertyChanged(); }
        //}

        private double _taxRate;
        public double TaxRate
        {
            get { return _taxRate; }
            set { _taxRate = value; RaisePropertyChanged(); }
        }

    }
}
