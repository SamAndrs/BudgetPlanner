using BudgetPlanner.DomainLayer.Models;
using BudgetPlanner.DomainLayer.Services;
using BudgetPlanner.PresentationLayer.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BudgetPlanner.PresentationLayer.ViewModels
{

    public class PrognosisViewVM : ViewModelBase
    {
        #region Fields + properties
        private UserSettingsService _settings;

        public UserSettingsService Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }


        private readonly BudgetPostService _budgetPostService;
        private readonly CategoryService _categoryService;
        private readonly PrognosisService _prognosisService;

        private Prognosis _selectedPrognosis;

        public Prognosis SelectedPrognosis
        {
            get { return _selectedPrognosis; }
            set
            {
                _selectedPrognosis = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TotalIncome));
                RaisePropertyChanged(nameof(TotalExpense));

                RaisePropertyChanged(nameof(AdjustedIncome));
                RecalculateIncome();

                RecalculateSummaries();
            }
        }

        private double _totalIncome;
        public double TotalIncome
        {
            get { return _totalIncome; }
            set { _totalIncome = value; RaisePropertyChanged(); }
        }


        public double TotalExpense => (double)SelectedPrognosis?.MonthlyExpense;
        public double ActualDifference => TotalIncome - TotalExpense;


        private double _adjustedIncome;
        public double AdjustedIncome
        {
            get { return _adjustedIncome; }
            set { _adjustedIncome = value; RaisePropertyChanged(); }
        }

        public double AdjustedDifference => AdjustedIncome - TotalExpense;


        public string ViewTitle { get; } = "Månadsprognos";

        // Collections
        public ObservableCollection<Prognosis> MonthlyPrognoses { get; set; } = new();
        public ObservableCollection<Category> Categories { get; private set; } = new();
        public ObservableCollection<RecurringBudgetPostTemplate> RecurringPosts { get; private set; } = new();

        // Income calculator
        public double TaxValue => _settings.TaxRate / 100;
        public double CalculatedMonthlyIncome { get; set; }  // Månadslön: Årsinkomst/ 12 * (1 - TaxValue)
        public double HourlySalaryActual { get; set; }    // Timlön (faktisk): Årsinkomst / årsarbetstid

        private int _hourlySalaryAdjustable;
        public int HourlySalaryAdjustable
        {
            get { return _hourlySalaryAdjustable; }
            set
            {
                _hourlySalaryAdjustable = value;
                RaisePropertyChanged();
                RecalculateIncome();
                RecalculateSummaries();
            }
        }

        public int HourlyIncomeMonthActual { get; set; } // Månadsinkomst (baserad på faktisk timlön)
        public int HourlyIncomeMonthAdjustable { get; set; } // Månadsinkomst (baserad på justerbar timlön)


        // Commands
        public ICommand SelectNextMonthCommand { get; }
        public ICommand SelectPreviousMonthCommand { get; }

        #endregion


        // ==============================================================================================================
        //       CONSTRUCTOR
        // ==============================================================================================================
        public PrognosisViewVM(UserSettingsService settings, BudgetPostService budgetPostService,
            CategoryService categoryService, PrognosisService prognosisService)
        {
            _settings = settings;
            _budgetPostService = budgetPostService;
            _categoryService = categoryService;
            _prognosisService = prognosisService;

            // Listen for income changes
            _settings.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(UserSettingsService.YearlyIncome) ||
                    e.PropertyName == nameof(UserSettingsService.YearlyWorkhours) ||
                    //e.PropertyName == nameof(UserSettingsService.WeeklyWorkhours) ||
                    e.PropertyName == nameof(UserSettingsService.TaxRate))
                {
                    RecalculateIncome();
                    RecalculateSummaries();
                }
            };

            LoadData();

            SelectNextMonthCommand = new DelegateCommand(_ => NextMonth());
            SelectPreviousMonthCommand = new DelegateCommand(_ => PreviousMonth());
        }


        // ==============================================================================================================
        //       METHODS
        // ==============================================================================================================
        private void LoadData()
        {
            // Load categories
            Categories = new ObservableCollection<Category>(_categoryService.GetAllCategories());
            RaisePropertyChanged(nameof(Categories));

            // Load prognosis for this year
            var year = DateTime.Now.Year;

            MonthlyPrognoses = new ObservableCollection<Prognosis>(
                _prognosisService.GetExistingPrognoses());

            var next = _prognosisService.GetOrCreateNextMonthPrognosis();
            MonthlyPrognoses.Add(next);

            RaisePropertyChanged(nameof(MonthlyPrognoses));

            SelectedPrognosis = MonthlyPrognoses.FirstOrDefault(p => p.FromDate.Month == DateTime.Now.Month);

            // Load list of recurring BudgetPosts (templates) for list.
            RecurringPosts = new ObservableCollection<RecurringBudgetPostTemplate>(_budgetPostService.GetRecurringTemplates());
        }

        private void RecalculateIncome()
        {

            // Calculate monthly income based on yearly income
            CalculatedMonthlyIncome = 
                (_settings.YearlyIncome / 12d) * (1 - TaxValue);  // Årsinkomst/ 12 * (1 - skattesats)

            // Calculate actual hourly salary based on yearly workhours
            HourlySalaryActual = 
                _settings.YearlyIncome / _settings.YearlyWorkhours;  // Inkomst per timme/ år (brutto)



            // 1. Calculate monthly income based on hourly salary actual
            HourlyIncomeMonthActual = 
                (int)(((HourlySalaryActual * _settings.YearlyWorkhours) * (1 - TaxValue)) / 12d);

            // 2. Calculate monthly income based on ADJUSTABLE hourly salary 
            HourlyIncomeMonthAdjustable = 
                (int)(((HourlySalaryAdjustable * _settings.YearlyWorkhours) * (1 - TaxValue)) / 12d);


            RaisePropertyChanged(nameof(CalculatedMonthlyIncome));
            RaisePropertyChanged(nameof(HourlySalaryActual));
            RaisePropertyChanged(nameof(HourlyIncomeMonthAdjustable));
            RaisePropertyChanged(nameof(HourlySalaryAdjustable));
        }

        private void RecalculateSummaries()
        {
            if (SelectedPrognosis == null)
            {
                TotalIncome = 0;
                AdjustedIncome = 0;
                return;
            }

            bool isNextMonth =
                    SelectedPrognosis.FromDate.Year == DateTime.Now.AddMonths(1).Year &&
                        SelectedPrognosis.FromDate.Month == DateTime.Now.AddMonths(1).Month;

            // Next month  (base incomes on calculator values
            if (isNextMonth)
            {
                TotalIncome = CalculatedMonthlyIncome + (double)SelectedPrognosis.MonthlyIncome;
                AdjustedIncome = HourlyIncomeMonthAdjustable + (double)SelectedPrognosis.MonthlyIncome;
            }

            // This month or earlier (base incomes on prognosis-object incomes value (historical)
            else
            {
                TotalIncome = (double)SelectedPrognosis.MonthlyIncome;
                AdjustedIncome = 0;
            }

            RaisePropertyChanged(nameof(TotalExpense));
            RaisePropertyChanged(nameof(ActualDifference));
            RaisePropertyChanged(nameof(AdjustedDifference));
        }

        private void PreviousMonth()
        {
            int index = MonthlyPrognoses.IndexOf(SelectedPrognosis);
            if (index > 0)
                SelectedPrognosis = MonthlyPrognoses[index - 1];

        }

        private void NextMonth()
        {
            int index = MonthlyPrognoses.IndexOf(SelectedPrognosis);
            if (index < MonthlyPrognoses.Count - 1)
                SelectedPrognosis = MonthlyPrognoses[index + 1];
        }
    }
}
