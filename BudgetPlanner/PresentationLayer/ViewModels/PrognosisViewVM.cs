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
                RaisePropertyChanged(nameof(TotalDifference));
            }
        }

        public decimal TotalIncome => SelectedPrognosis?.MonthlyIncome ?? 0;
        public decimal TotalExpense => SelectedPrognosis?.MonthlyExpense ?? 0;
        public decimal TotalDifference => SelectedPrognosis?.TotalSum ?? 0;

        public string ViewTitle { get; } = "Månadsprognos";

        // Collections
        public ObservableCollection<Prognosis> MonthlyPrognoses { get; set; } = new();
        public ObservableCollection<Category> Categories { get; private set; } = new();
        public ObservableCollection<RecurringBudgetPostTemplate> RecurringPosts { get; private set; } = new();

        // Income calculator
        public double CalculatedMonthlyIncome { get; set; }
        public double HourlyIncomeYear { get; set; }
        public double HourlyIncomeMonth { get; set; }

        // Commands
        public ICommand SelectNextMonthCommand { get; }
        public ICommand SelectPreviousMonthCommand { get; }

        #endregion


        // Constructor
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
                    e.PropertyName == nameof(UserSettingsService.WeeklyWorkhours))
                {
                    RecalculateIncome();
                }
            };

            LoadData();

            SelectNextMonthCommand = new DelegateCommand(_ => NextMonth());
            SelectPreviousMonthCommand = new DelegateCommand(_ => PreviousMonth());
        }


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
            CalculatedMonthlyIncome = _settings.YearlyIncome / 12;
            HourlyIncomeYear = _settings.YearlyIncome / _settings.YearlyWorkhours;
            HourlyIncomeMonth = CalculatedMonthlyIncome / _settings.WeeklyWorkhours;

            RaisePropertyChanged(nameof(CalculatedMonthlyIncome));
            RaisePropertyChanged(nameof(HourlyIncomeYear));
            RaisePropertyChanged(nameof(HourlyIncomeMonth));
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
