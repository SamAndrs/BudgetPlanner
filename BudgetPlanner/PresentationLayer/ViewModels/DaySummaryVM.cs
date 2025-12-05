using BudgetPlanner.DomainLayer.Models;
using System.Globalization;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    /*
			Used on Dashboeard View to summarize income and expense per day in month square grid
			and list.
	 */
    public class DaySummaryVM : ViewModelBase
    {
		private int _dayNumber;

		public int DayNumber
		{
			get { return _dayNumber; }
			set => SetProperty(ref _dayNumber, value);
        }

		private double _totalIncome;

		public double TotalIncome
		{
			get { return _totalIncome; }
            set => SetProperty(ref _totalIncome, value);
        }

		private double _totalExpense;

		public double TotalExpense
		{
			get { return _totalExpense; }
            set => SetProperty(ref _totalExpense, value);
        }

		public double NetDay => TotalIncome - TotalExpense;


        public bool HasIncome => TotalIncome > 0;
		public bool HasExpense => TotalExpense > 0;
		public bool IsZero => TotalIncome == 0 && TotalExpense == 0;

		public bool IsPlaceholder { get; set; } = false; // used to handle "empty days" (spots) in month grid

		public IEnumerable<BudgetPost> Posts { get; set; } = Enumerable.Empty<BudgetPost>();

        public DateTime Date { get; set; }
		public string FormattedDate => Date.ToString("dd MMMM yyyy", new CultureInfo("sv-SE"));
    }
}
