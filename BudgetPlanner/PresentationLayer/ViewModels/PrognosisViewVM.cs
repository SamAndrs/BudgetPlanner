using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using BudgetPlanner.PresentationLayer.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class PrognosisViewVM : ViewModelBase
    {
        #region Calculation Properties
        private double _yearlyIncome;

        public double YearlyIncome
        {
            get { return _yearlyIncome; }
            set
            {
                _yearlyIncome = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CalculatedMonthlyIncome));
            }
        }

        private double _yearlyWorkhours;

        public double YearlyWorkHours
        {
            get { return _yearlyWorkhours; }
            set
            {
                _yearlyWorkhours = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(CalculatedMonthlyIncome));
            }
        }


        public double CalculatedMonthlyIncome
        {
            get
            {
                if (YearlyWorkHours <= 0) return 0;
                double hourlyRate = YearlyIncome / YearlyWorkHours;
                return hourlyRate * (YearlyWorkHours / 12);
            }

        }


        #endregion


        #region PROGNOSIS Properties
        public ObservableCollection<BudgetPost> AllPosts { get; set; } = new();
        public ObservableCollection<Prognosis> MonthlyPrognoses { get; set; } = new();

        public decimal TotalIncome => SelectedPrognosis?.TotalIncome ?? 0;
        public decimal TotalExpense => SelectedPrognosis?.TotalExpenses ?? 0;

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

        #endregion
        public ICommand SelectNextMonthCommand { get; }
        public ICommand SelectPreviousMonthCommand { get; }



        // Constructor
        public PrognosisViewVM()
        {
            // Mock data
            LoadMockData();

            //TODO: Hämta data från service
            

            GeneratePrognosesForYear(DateTime.Now.Year);

            SelectedPrognosis = MonthlyPrognoses
                .FirstOrDefault(p => p.FromDate.Month == DateTime.Now.Month);

            SelectNextMonthCommand = new DelegateCommand(NextMonth);
            SelectPreviousMonthCommand = new DelegateCommand(PreviousMonth);
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


        // Mockdata for demo
        private void LoadMockData()
        {
            YearlyIncome = 200000;
            YearlyWorkHours = 1920;

            var salaryCategory = new Category { Id = 1, Name = "Lön" };
            var housingCategory = new Category { Id = 2, Name = "Boende" };
            var otherCategory = new Category { Id = 3, Name = "Övrigt" };

            AllPosts = new ObservableCollection<BudgetPost>
        {
            // --- ÅTERKOMMANDE INKOMSTER ---
        new BudgetPost {
            Id = 1, Amount = 35000, Description = "Lön",
            CategoryId = 1, Category = salaryCategory,
            Recurring = Recurring.Monthly, PostType = BudgetPostType.Income
        },

        // --- ÅTERKOMMANDE UTGIFTER ---
        new BudgetPost {
            Id = 2, Amount = 8500, Description = "Hyra",
            CategoryId = 2, Category = housingCategory,
            Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense
        },
        new BudgetPost {
            Id = 3, Amount = 399, Description = "Bredband",
            CategoryId = 2, Category = housingCategory,
            Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense
        },
        new BudgetPost {
            Id = 4, Amount = 299, Description = "Gymkort",
            CategoryId = 3, Category = otherCategory,
            Recurring = Recurring.Monthly, PostType = BudgetPostType.Expense
        },

        // --- ENGÅNGSPOSTER PER SPECIFIK MÅNAD ---
        // Februari: Bilservice
        new BudgetPost {
            Id = 5, Amount = 3200, Description = "Bilservice",
            CategoryId = 3, Category = otherCategory,
            Recurring = Recurring.None, PostType = BudgetPostType.Expense,
            Date = new DateTime(DateTime.Now.Year, 2, 5)
        },

        // Mars: Bonus
        new BudgetPost {
            Id = 6, Amount = 8000, Description = "Bonus",
            CategoryId = 1, Category = salaryCategory,
            Recurring = Recurring.None, PostType = BudgetPostType.Income,
            Date = new DateTime(DateTime.Now.Year, 3, 25)
        },

        // Maj: Försäkring
        new BudgetPost {
            Id = 7, Amount = 1200, Description = "Husförsäkring",
            CategoryId = 2, Category = housingCategory,
            Recurring = Recurring.None, PostType = BudgetPostType.Expense,
            Date = new DateTime(DateTime.Now.Year, 5, 10)
        },

        // December: Julklappar
        new BudgetPost {
            Id = 8, Amount = 4500, Description = "Julklappar",
            CategoryId = 3, Category = otherCategory,
            Recurring = Recurring.None, PostType = BudgetPostType.Expense,
            Date = new DateTime(DateTime.Now.Year, 12, 18)
        }
    };

        }
    }
}

/*
  <!-- 
           Syfte:
        Visa beräkning av nästa månads budget baserat på återkommande poster.

⭐ Innehåll:

Kort med:

”Förväntade inkomster”

”Förväntade utgifter”

”Prognostiserat saldo”

Lista: ”Återkommande poster som påverkar nästa månad”

Möjlighet att justera:

Frånvaro

Extra inkomster

Engångskostnader

MVVM-idé:

PrognosisService → CalculateNextMonthPrognosis()

Spara resultat i Prognosis-tabell
        -->
 
 */
