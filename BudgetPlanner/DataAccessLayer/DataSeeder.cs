using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.DataAccessLayer
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            var now = DateTime.Now;

            if (!context.Prognoses.Any())
            {
                // Lista med månader att skapa prognoser för (nu + 5 månader bakåt)
                var months = Enumerable.Range(0, 6)
                    .Select(i => now.AddMonths(-i))
                    .ToList();

                foreach (var date in months)
                {
                    var prognosis = new Prognosis
                    {
                        Id = Guid.NewGuid(),
                        FromDate = new DateTime(date.Year, date.Month, 1),
                        ToDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)),
                        Month = date.ToString("MMMM yyyy"), // <-- Formaterad Month
                        MonthlyIncome = 0m,  // Du kan sätta defaultvärden här
                        MonthlyExpense = 0m,
                        TotalSum = 0m,
                        BudgetPosts = new List<BudgetPost>()
                    };

                    // Lägg till recurring poster
                    AddRecurringBudgetPosts(prognosis, date);

                    // Lägg till non-recurring poster
                    AddNonRecurringBudgetPosts(prognosis, date);

                    context.Prognoses.Add(prognosis);
                }

                await context.SaveChangesAsync();
            }
        }

        private static void AddRecurringBudgetPosts(Prognosis prognosis, DateTime monthDate)
        {
            prognosis.BudgetPosts.AddRange(new List<BudgetPost>
            {
            new BudgetPost { Amount = 28500, CategoryId = 14, Description = "Lön", Recurring = Recurring.Monthly, Date = new DateTime(monthDate.Year, monthDate.Month, 23), PostType = BudgetPostType.Income },
            new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(monthDate.Year, monthDate.Month, 25), PostType = BudgetPostType.Expense },
            new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(monthDate.Year, monthDate.Month, 1), PostType = BudgetPostType.Expense },
            new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(monthDate.Year, monthDate.Month, 6), PostType = BudgetPostType.Expense },
            new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(monthDate.Year, monthDate.Month, 5), PostType = BudgetPostType.Income },
            new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(monthDate.Year, monthDate.Month, 1), PostType = BudgetPostType.Expense },
            new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(monthDate.Year, monthDate.Month, 1), PostType = BudgetPostType.Expense },
            });
        }

        private static void AddNonRecurringBudgetPosts(Prognosis prognosis, DateTime monthDate)
        {
            var random = new Random();

            // 15 möjliga budgetposter
            var possiblePosts = new List<BudgetPost>
            {
                new BudgetPost { CategoryId = 1, Description = "Biobesök", Amount = 500, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 2, Description = "Restaurang", Amount = 450, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 3, Description = "Tanka bilen", Amount = 600, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 4, Description = "Födelsedagspresent", Amount = 1200, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 5, Description = "Klippning", Amount = 350, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 6, Description = "Rörmokare", Amount = 5500, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 7, Description = "Kaffe med vänner", Amount = 100, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 8, Description = "Barnaktiviteter", Amount = 300, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 9, Description = "Träning gym", Amount = 250, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 10, Description = "Kattleksak extra", Amount = 300, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 11, Description = "Netflix extrakostnad", Amount = 150, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 12, Description = "Veterinärkontroll", Amount = 1200, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 13, Description = "Biobesök extra", Amount = 200, PostType = BudgetPostType.Expense, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 14, Description = "Bonus", Amount = 1200, PostType = BudgetPostType.Income, Recurring = Recurring.None },
                new BudgetPost { CategoryId = 15, Description = "Extrajobb helg", Amount = 1500, PostType = BudgetPostType.Income, Recurring = Recurring.None }
            };

            // Slumpa fram 6 poster utan dubbletter
            var selectedPosts = possiblePosts.OrderBy(x => random.Next()).Take(6).ToList();

            // Sätt datum för varje post slumpmässigt inom månaden
            foreach (var post in selectedPosts)
            {
                int day = random.Next(1, DateTime.DaysInMonth(monthDate.Year, monthDate.Month) + 1);
                post.Date = new DateTime(monthDate.Year, monthDate.Month, day);
                prognosis.BudgetPosts.Add(post);
            }
        }


    }

    #region
    /*
    public static async Task SeedAsync(AppDbContext context)
    {
        var now = DateTime.Now;

        if (!context.Prognoses.Any())
        {
            var p1 = new Prognosis
            {
                Id = Guid.NewGuid(),
                FromDate = new DateTime(now.Year, now.Month, 1),
                ToDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)),
                Month = $"{now.Year}-{now.Month:D2}",
                MonthlyIncome = 30600m,
                MonthlyExpense = 20510m,
                TotalSum = 10090m,
                BudgetPosts = new List<BudgetPost>
            {
                new BudgetPost { Amount = 28500, CategoryId = 14, Description = "Lön", Recurring = Recurring.Monthly, Date = new DateTime(now.Year, now.Month, 23), PostType = BudgetPostType.Income },
                new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(now.Year, now.Month, 25), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(now.Year, now.Month, 1), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(now.Year, now.Month, 6), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.Year, now.Month, 5), PostType = BudgetPostType.Income },
                new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(now.Year, now.Month, 1), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(now.Year, now.Month, 1), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 500, CategoryId = 13, Description = "Biobesök", Recurring = Recurring.None, Date = new DateTime(now.Year, now.Month, 15), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 1200, CategoryId = 15, Description = "Bonus", Recurring = Recurring.None, Date = new DateTime(now.Year, now.Month, 15), PostType = BudgetPostType.Income },
                new BudgetPost { Amount = 480, CategoryId = 2, Description = "Restaurang", Recurring = Recurring.None, Date = new DateTime(now.Year, now.Month, 8), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 180, CategoryId = 2, Description = "McDonalds", Recurring = Recurring.None, Date = new DateTime(now.Year, now.Month, 26), PostType = BudgetPostType.Expense }
            }
            };

            var p2 = new Prognosis
            {
                Id = Guid.NewGuid(),
                FromDate = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 1),
                ToDate = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, DateTime.DaysInMonth(now.AddMonths(-1).Year, now.AddMonths(-1).Month)),
                Month = $"{now.AddMonths(-1).Year}-{now.AddMonths(-1).Month:D2}",
                MonthlyIncome = 30200m,
                MonthlyExpense = 23230m,
                TotalSum = 6970m,
                BudgetPosts = new List<BudgetPost>
            {
                new BudgetPost { Amount = 28500, CategoryId = 14, Description = "Lön", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 23), PostType = BudgetPostType.Income },
                new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 25), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 1), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 6), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 5), PostType = BudgetPostType.Income },
                new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 1), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 1), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 1200, CategoryId = 12, Description = "Veterinärkontroll", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 12), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 800, CategoryId = 16, Description = "Frilansjobb", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 18), PostType = BudgetPostType.Income },
                new BudgetPost { Amount = 2500, CategoryId = 3, Description = "Bildelar", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 22), PostType = BudgetPostType.Expense },
                new BudgetPost { Amount = 180, CategoryId = 2, Description = "McDonalds", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 5), PostType = BudgetPostType.Expense }
            }
            };

            var p3 = new Prognosis
            {
                Id = Guid.NewGuid(),
                FromDate = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 1),
                ToDate = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, DateTime.DaysInMonth(now.AddMonths(-2).Year, now.AddMonths(-2).Month)),
                Month = $"{now.AddMonths(-2).Year}-{now.AddMonths(-2).Month:D2}",
                MonthlyIncome = 30900m,
                MonthlyExpense = 20930m,
                TotalSum = 9970m,
                BudgetPosts = new List<BudgetPost>
                {
                    // RECURRING
                    new BudgetPost { Amount = 28500, CategoryId = 14, Description = "Lön", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 23), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 25), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 1), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 6), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 5), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 1), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 1), PostType = BudgetPostType.Expense },

                    // NON-RECURRING
                    new BudgetPost { Amount = 200, CategoryId = 13, Description = "Biobesök", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 14), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1500, CategoryId = 16, Description = "Extrajobb helg", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 20), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 180, CategoryId = 2, Description = "McDonalds", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 14), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, CategoryId = 4, Description = "Födelsedagspresent", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 8), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 500, CategoryId = 3, Description = "Tanka bilen", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 8), PostType = BudgetPostType.Expense },
                }
            };

            var p4 = new Prognosis
            {
                Id = Guid.NewGuid(),
                FromDate = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 1),
                ToDate = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, DateTime.DaysInMonth(now.AddMonths(-3).Year, now.AddMonths(-3).Month)),
                Month = $"{now.AddMonths(-3).Year}-{now.AddMonths(-3).Month:D2}",
                MonthlyIncome = 29400m,
                MonthlyExpense = 26230m,
                TotalSum = 3170m,
                BudgetPosts = new List<BudgetPost>
                {
                    // RECURRING
                    new BudgetPost { Amount = 28500, CategoryId = 14, Description = "Lön", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 23), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 25), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 1), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 6), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 5), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 1), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 1), PostType = BudgetPostType.Expense },

                    // NON-RECURRING
                    new BudgetPost { Amount = 400, CategoryId = 4, Description = "Nya strumpor", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 10), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 500, CategoryId = 13, Description = "Biobesök", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 15), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 480, CategoryId = 2, Description = "Restaurang", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 18), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 5500, CategoryId = 6, Description = "Rörmokaren", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 24), PostType = BudgetPostType.Expense },
                }
            };

            var p5 = new Prognosis
            {
                Id = Guid.NewGuid(),
                FromDate = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 1),
                ToDate = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, DateTime.DaysInMonth(now.AddMonths(-4).Year, now.AddMonths(-4).Month)),
                Month = $"{now.AddMonths(-4).Year}-{now.AddMonths(-4).Month:D2}",
                MonthlyIncome = 30000m,
                MonthlyExpense = 20200m,
                TotalSum = 9800m,
                BudgetPosts = new List<BudgetPost>
                {
                    // RECURRING …
                    new BudgetPost { Amount = 28500, CategoryId = 14, Description = "Lön", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 23), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 25), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 1), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 6), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 5), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 1), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 1), PostType = BudgetPostType.Expense },

                    // NON-RECURRING
                    new BudgetPost { Amount = 300, CategoryId = 8, Description = "Barnaktiviteter", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 8), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 600, CategoryId = 16, Description = "Säljtjänst", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 17), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 250, CategoryId = 12, Description = "Kattleksak", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 10), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 300, CategoryId = 3, Description = "Tanka bilen", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 8), PostType = BudgetPostType.Expense },
                }
            };

            var p6 = new Prognosis
            {
                Id = Guid.NewGuid(),
                FromDate = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 1),
                ToDate = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, DateTime.DaysInMonth(now.AddMonths(-5).Year, now.AddMonths(-5).Month)),
                Month = $"{now.AddMonths(-5).Year}-{now.AddMonths(-5).Month:D2}",
                MonthlyIncome = 30000m,
                MonthlyExpense = 20330m,
                TotalSum = 9660m,
                BudgetPosts = new List<BudgetPost>
                {
                    // RECURRING
                    new BudgetPost { Amount = 28500, CategoryId = 14, Description = "Lön", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 23), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 25), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 1), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 6), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 5), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 1), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 1), PostType = BudgetPostType.Expense },

                    // NON-RECURRING
                    new BudgetPost { Amount = 480, CategoryId = 2, Description = "McDonalds", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 9), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 300, CategoryId = 10, Description = "Kattleksak extra", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 12), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 200, CategoryId = 13, Description = "Biobesök extra", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 6), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 600, CategoryId = 16, Description = "Säljuppdrag", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 21), PostType = BudgetPostType.Income },
                }
            };


            var prognoses = new List<Prognosis> { p1, p2, p3, p4, p5, p6 };
            await context.Prognoses.AddRangeAsync(prognoses);
            await context.SaveChangesAsync();
        }
    }  */
    #endregion
}



