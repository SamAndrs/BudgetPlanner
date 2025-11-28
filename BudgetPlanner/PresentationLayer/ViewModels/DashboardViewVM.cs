using System.Collections.ObjectModel;
using System.Diagnostics;
using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;

namespace BudgetPlanner.PresentationLayer.ViewModels
{
    public class DashboardViewVM : ViewModelBase
    {

        public string? Title { get; set; }

        public ObservableCollection<BudgetPost> RecentPosts { get; set; }
        public double TotalIncomeThisMonth { get; set; }
        public double TotalExpensesThisMonth { get; set; }
        public double NetBalanceThisMonth => TotalIncomeThisMonth - TotalExpensesThisMonth;

        public IReadOnlyList<CategorySummary> CategorySummaries { get; set; }

        public DashboardViewVM()
        {

            var Categories = new[]
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

            RecentPosts = new ObservableCollection<BudgetPost>
            {
                new BudgetPost { Id = 1, Amount = 3200, Category = Categories[1], CategoryId= Categories[1].Id, Description = "Veckohandling", Date = DateTime.UtcNow.AddDays(-2), Recurring= Recurring.Weekly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 2, Amount = 8500, Category = Categories[5], CategoryId= Categories[5].Id, Description = "Månadshyra", Date = DateTime.UtcNow.AddDays(-10), Recurring= Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 3, Amount = 28500, Category = Categories[13], CategoryId= Categories[13].Id, Description = "Lön för Juni", Date = DateTime.UtcNow.AddDays(-15), Recurring= Recurring.Monthly, PostType = BudgetPostType.Income },
                new BudgetPost { Id = 4, Amount = 1200, Category = Categories[2], CategoryId= Categories[2].Id, Description = "Busskort", Date = DateTime.UtcNow.AddDays(-5), Recurring= Recurring.Monthly, PostType = BudgetPostType.Expense },
                new BudgetPost { Id = 5, Amount = 500, Category = Categories[12], CategoryId= Categories[12].Id, Description = "Biobesök", Date = DateTime.UtcNow.AddDays(-7), Recurring= Recurring.None, PostType = BudgetPostType.Expense }
            };

            TotalIncomeThisMonth = 28500;
            TotalExpensesThisMonth = 3200 + 8500 + 1200 + 500;

            CategorySummaries = new List<CategorySummary>
            {
                new CategorySummary("Mat", 3200),
                new CategorySummary("Transport", 1200),
                new CategorySummary("Lön", 28500),
                new CategorySummary("Underhållning", 500),
                new CategorySummary("Hem", 8500)
            };
        }

        public record CategorySummary(string CategoryName, double TotalAmount);
    }
}

/*
 <!-- 
Innehåll:

Kort med: ”Totala inkomster denna månad”

Kort med: ”Totala utgifter denna månad”

Kort med: ”Saldo”

Mini-prognoswidget: ”Nästa månad: +/− XX kr”

Pie chart: Utgifter per kategori

Snabblänkar: ”Lägg till utgift”, ”Lägg till inkomst”

🧩 WPF UI-komponenter:

ui:Card

ui:ProgressRing

ui:SymbolIcon

NavigationViewItem för routing
-->
 */