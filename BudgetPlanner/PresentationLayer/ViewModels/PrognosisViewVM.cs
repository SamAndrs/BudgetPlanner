using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using BudgetPlanner.DomainLayer.Services;
using BudgetPlanner.PresentationLayer.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class PrognosisViewVM : ViewModelBase
    {
        private readonly UserSettingsService _settings;
        public UserSettingsService Settings => _settings;

        private double _calculatedMonthlyIncome;
        public double CalculatedMonthlyIncome
        {
            get { return _calculatedMonthlyIncome; }
            set { _calculatedMonthlyIncome = value; RaisePropertyChanged(); }
        }


        private double _hourlyIncomeYear;
        public double HourlyIncomeYear
        {
            get { return _hourlyIncomeYear; }
            set { _hourlyIncomeYear = value; RaisePropertyChanged(); }
        }

        private double _hourlyIncomeMonth;

        public double HourlyIncomeMonth
        {
            get { return _hourlyIncomeMonth; }
            set { _hourlyIncomeMonth = value; RaisePropertyChanged(); }
        }




        public string ViewTitle { get; } = "Månadsprognos";

        // Collections
        public ObservableCollection<BudgetPost> AllPosts { get; set; } = new();
        public ObservableCollection<Prognosis> MonthlyPrognoses { get; set; } = new();
        public ObservableCollection<Category> Categories { get; set; }

        public decimal TotalIncome => SelectedPrognosis?.TotalIncome ?? 0;
        public decimal TotalExpense => SelectedPrognosis?.TotalExpenses ?? 0;
        public decimal TotalDifference => TotalIncome - TotalExpense;

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
            }
        }


        // Commands
        public ICommand SelectNextMonthCommand { get; }
        public ICommand SelectPreviousMonthCommand { get; }
        

        // Constructor
        public PrognosisViewVM(UserSettingsService settings)
        {
            _settings = settings;


            _settings.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(UserSettingsService.YearlyIncome) ||
                    e.PropertyName == nameof(UserSettingsService.YearlyWorkhours) ||
                    e.PropertyName == nameof(UserSettingsService.WeeklyWorkhours))
                {
                    RecalculateIncome();
                }
            };

            // Mock data  //TODO: Hämta data från service ===============
            Categories = LoadCategories();
            AllPosts = LoadData();

            GeneratePrognosesForYear(DateTime.Now.Year);
            //GeneratePrognosisRange(DateTime.Now); // TODO: Uncomment after new methods

            SelectedPrognosis = MonthlyPrognoses
               .FirstOrDefault(p => p.FromDate.Month == DateTime.Now.Month);

            SelectNextMonthCommand = new DelegateCommand(NextMonth);
            SelectPreviousMonthCommand = new DelegateCommand(PreviousMonth);

        }       

        private void RecalculateIncome()
        {
            CalculatedMonthlyIncome = _settings.YearlyIncome / 12;
            HourlyIncomeYear = _settings.YearlyIncome / _settings.YearlyWorkhours;
            HourlyIncomeMonth = CalculatedMonthlyIncome / _settings.WeeklyWorkhours;
        }


        // MAIN LOGIC: Generate 10 monthly prognosis objects
        private void GeneratePrognosesForYear(int year)
        {
            MonthlyPrognoses.Clear();

            for (int month = 1; month <= 12; month++)
            {
                var from = new DateTime(year, month, 1);
                var to = from.AddMonths(1).AddDays(-1);

                var prognosis = new Prognosis
                {
                    Id = month,
                    Month = $"{from:MMMM}",
                    FromDate = from,
                    ToDate = to,
                };

                CalculateMonthlyPrognosis(prognosis);

                MonthlyPrognoses.Add(prognosis);
            }
        }

        // Monthly calculation logic
        private void CalculateMonthlyPrognosis(Prognosis prognosis)
        {
            prognosis.TotalIncome = 0;
            prognosis.TotalExpenses = 0;
            prognosis.BudgetPosts.Clear();

            // Hämta alla poster för nuvarande månad
            var postsForMonth = AllPosts.Where(p =>
            (p.Recurring == Recurring.None && p.Date?.Month == prognosis.FromDate.Month) ||
            (p.Recurring != Recurring.None));

            foreach (var post in postsForMonth)
            {
                // Beräkna månadsvärde (0 för engångsposter innan filtrering)
                decimal monthlyAmount = post.Recurring == Recurring.None
                       ? (decimal)post.Amount
                       : ConvertRecurringToMonthly(post);

                if (post.PostType == BudgetPostType.Income)
                    prognosis.TotalIncome += monthlyAmount;

                else
                    prognosis.TotalExpenses += Math.Abs(monthlyAmount);

                prognosis.BudgetPosts.Add(post);
            }

            prognosis.TotalSum = prognosis.TotalIncome - prognosis.TotalExpenses;
        }


        // Recurring conversion helper method
        private decimal ConvertRecurringToMonthly(BudgetPost post)
        {
            var amount = (decimal)post.Amount;

            return post.Recurring switch
            {
                Recurring.Daily => amount * 30,
                Recurring.Weekly => amount * 4.33m,
                Recurring.Monthly => amount,
                Recurring.Yearly => amount / 12,
                _ => 0
            };
        }

        // Month navigation
        private void NextMonth(object? obj)
        {
            int index = MonthlyPrognoses.IndexOf(SelectedPrognosis);
            if (index < MonthlyPrognoses.Count - 1)
                SelectedPrognosis = MonthlyPrognoses[index + 1];
        }

        private void PreviousMonth(object? obj)
        {
            int index = MonthlyPrognoses.IndexOf(SelectedPrognosis);
            if (index > 0)
                SelectedPrognosis = MonthlyPrognoses[index - 1];
        }


        private ObservableCollection<BudgetPost> LoadData()
        {
            var now = DateTime.Now;

            return new ObservableCollection<BudgetPost>
            {
                // ==== Nuvarande månad ====
                new BudgetPost { Id = 1, Amount = 3200, Category = Categories[1], CategoryId= Categories[1].Id, Description = "Veckohandling", Date = new DateTime(now.Year, now.Month, 1), Recurring= Recurring.Weekly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 2, Amount = 8500, Category = Categories[5], CategoryId= Categories[5].Id, Description = "Månadshyra", Date = new DateTime(now.Year, now.Month, 25), Recurring= Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 3, Amount = 28500, Category = Categories[13], CategoryId= Categories[13].Id, Description = "Lön för Juni", Date = new DateTime(now.Year, now.Month, 22), Recurring= Recurring.Monthly, PostType = BudgetPostType.Income },
                new BudgetPost { Id = 4, Amount = 1200, Category = Categories[2], CategoryId= Categories[2].Id, Description = "Busskort", Date = new DateTime(now.Year, now.Month, 1), Recurring= Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 5, Amount = 500, Category = Categories[12], CategoryId= Categories[12].Id, Description = "Biobesök", Date = new DateTime(now.Year, now.Month, 15), Recurring= Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 6, Amount = 1200, Category = Categories[14], CategoryId= Categories[14].Id, Description = "Bonus", Date = new DateTime(now.Year, now.Month, 15), Recurring= Recurring.None, PostType = BudgetPostType.Income },
            
                // ===== Månad 2: En månad bakåt  =====
                new BudgetPost { Id = 1, Amount = 3200, Category = Categories[0], CategoryId = 2, Description = "Veckohandling", Date = new DateTime(now.Year, now.Month-1, 1), Recurring = Recurring.Weekly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 2, Amount = 900, Category = Categories[1], CategoryId = 3, Description = "Månadskort buss", Date = new DateTime(now.Year, now.Month-1, 2), Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 3, Amount = 8500, Category = Categories[4], CategoryId = 6, Description = "Hyra", Date = new DateTime(now.Year, now.Month-1, 25), Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 4, Amount = 1200, Category = Categories[10], CategoryId = 12, Description = "Veterinärkontroll", Date = new DateTime(now.Year, now.Month-1, 12), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 5, Amount = 28500, Category = Categories[12], CategoryId = 14, Description = "Lön", Date = new DateTime(now.Year, now.Month-1, 23), Recurring = Recurring.Monthly, PostType = BudgetPostType.Income },
                new BudgetPost { Id = 6, Amount = 800, Category = Categories[14], CategoryId = 16, Description = "Frilansjobb", Date = new DateTime(now.Year, now.Month-1, 18), Recurring = Recurring.None, PostType = BudgetPostType.Income },

                // ===== Månad 3: Två månader bakåt =====
                new BudgetPost { Id = 7, Amount = 3100, Category = Categories[0], CategoryId = 2, Description = "Storhandling", Date = new DateTime(now.Year, now.Month - 2, 3), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 8, Amount = 750, Category = Categories[1], CategoryId = 3, Description = "Resor till arbete", Date = new DateTime(now.Year, now.Month - 2, 6), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 9, Amount = 8500, Category = Categories[4], CategoryId = 6, Description = "Hyra", Date = new DateTime(now.Year, now.Month - 2, 25), Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 10, Amount = 200, Category = Categories[11], CategoryId = 13, Description = "Biobesök", Date = new DateTime(now.Year, now.Month - 2, 14), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 11, Amount = 28500, Category = Categories[12], CategoryId = 14, Description = "Lön", Date = new DateTime(now.Year, now.Month - 2, 23), Recurring = Recurring.Monthly, PostType = BudgetPostType.Income },
                new BudgetPost { Id = 12, Amount = 1500, Category = Categories[15], CategoryId = 16, Description = "Extrajobb helg", Date = new DateTime(now.Year, now.Month - 2, 20), Recurring = Recurring.None, PostType = BudgetPostType.Income },

                // ===== Månad 4: Tre månader bakåt =====
                new BudgetPost { Id = 13, Amount = 2800, Category = Categories[0], CategoryId = 2, Description = "Matinköp", Date = new DateTime(now.Year, now.Month - 3, 4), Recurring = Recurring.Weekly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 14, Amount = 8500, Category = Categories[4], CategoryId = 6, Description = "Hyra", Date = new DateTime(now.Year, now.Month - 3, 25), Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 15, Amount = 1200, Category = Categories[9], CategoryId = 11, Description = "Netflix + Spotify", Date = new DateTime(now.Year, now.Month - 3, 1), Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 16, Amount = 400, Category = Categories[2], CategoryId = 4, Description = "Nya strumpor", Date = new DateTime(now.Year, now.Month - 3, 10), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 17, Amount = 28500, Category = Categories[12], CategoryId = 14, Description = "Lön", Date = new DateTime(now.Year, now.Month - 3, 23), Recurring = Recurring.Monthly, PostType = BudgetPostType.Income },
                new BudgetPost { Id = 18, Amount = 900, Category = Categories[13], CategoryId = 15, Description = "Studiebidrag", Date = new DateTime(now.Year, now.Month - 3, 5), Recurring = Recurring.Monthly, PostType = BudgetPostType.Income },

                // ===== Månad 5: Fyra månader bakåt =====
                new BudgetPost { Id = 19, Amount = 3000, Category = Categories[0], CategoryId = 2, Description = "Matkasse", Date = new DateTime(now.Year, now.Month - 4, 6), Recurring = Recurring.Weekly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 20, Amount = 600, Category = Categories[1], CategoryId = 3, Description = "Bussbiljetter", Date = new DateTime(now.Year, now.Month - 4, 12), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 21, Amount = 8500, Category = Categories[4], CategoryId = 6, Description = "Hyra", Date = new DateTime(now.Year, now.Month - 4, 25), Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 22, Amount = 300, Category = Categories[6], CategoryId = 8, Description = "Barnaktiviteter", Date = new DateTime(now.Year, now.Month - 4, 8), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 23, Amount = 28500, Category = Categories[12], CategoryId = 14, Description = "Lön", Date = new DateTime(now.Year, now.Month - 4, 23), Recurring = Recurring.Monthly, PostType = BudgetPostType.Income },
                new BudgetPost { Id = 24, Amount = 600, Category = Categories[14], CategoryId = 16, Description = "Säljtjänst", Date = new DateTime(now.Year, now.Month - 4, 17), Recurring = Recurring.None, PostType = BudgetPostType.Income },
            };
        }

        private ObservableCollection<Category> LoadCategories()
        {
            return new ObservableCollection<Category>
            {
               // Expenses
                new Category { Id = 1, Name = "Alla" },
                new Category { Id = 2, Name = "Mat" },
                new Category { Id = 3, Name = "Transport" },
                new Category { Id = 4, Name = "Kläder" },
                new Category { Id = 5, Name = "Skatt" },
                new Category { Id = 6, Name = "Hem" },
                new Category { Id = 7, Name = "Hobby" },
                new Category { Id = 8, Name = "Barn" },
                new Category { Id = 9, Name = "TV" },
                new Category { Id = 10, Name = "SaaS" },
                new Category { Id = 11, Name = "Prenumerationer" },
                new Category { Id = 12, Name = "Husdjur" },
                new Category { Id = 13, Name = "Underhållning" },

                // Income
                new Category { Id = 14, Name = "Lön" },
                new Category { Id = 15, Name = "Bidrag" },
                new Category { Id = 16, Name = "Extrainkomst" },

                new Category { Id = 17, Name = "Okänd" }
            };
        }
    }
}
