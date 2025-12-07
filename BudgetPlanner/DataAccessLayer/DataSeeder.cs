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
                var p1 = new Prognosis
                {
                    Id = Guid.NewGuid(),
                    FromDate = new DateTime(now.Year, now.Month, 1),
                    ToDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)),
                    Month = new DateTime(now.Year, now.Month, 1).ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture),
                    MonthlyIncome = 30600m,
                    MonthlyExpense = 20510m,
                    TotalSum = 10090m,
                    BudgetPosts = new List<BudgetPost>
                    {
                         new BudgetPost { Amount = 2500, CategoryId = 8, Description = "Barnbidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.Year, now.Month, 23), PostType = BudgetPostType.Income, RecurringId = new Guid("389904d6-c94c-4bd8-8864-27644219f5ad") },
                       new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(now.Year, now.Month, 25), PostType = BudgetPostType.Expense, RecurringId = new Guid("704d48ad-aa50-44fd-a0ca-8f0b59266f30") },
                       new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(now.Year, now.Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("783157fe-c960-43cc-b490-66ac6ca8fad7") },
                        new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(now.Year, now.Month, 6), PostType = BudgetPostType.Expense, RecurringId = new Guid("30d8676b-de2f-4cf9-90af-18adf034feb5") },
                        new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.Year, now.Month, 5), PostType = BudgetPostType.Income, RecurringId = new Guid("5a36fbcd-47f9-4753-9c16-91d1dbd9eaab") },
                        new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(now.Year, now.Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("810421e0-9578-4959-a5e6-0af399ab0cdf") },
                        new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(now.Year, now.Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("19cf7680-db76-4047-bb8b-79217df278c1") },

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
                    Month = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 1).ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture),
                    MonthlyIncome = 30200m,
                    MonthlyExpense = 23230m,
                    TotalSum = 6970m,
                    BudgetPosts = new List<BudgetPost>
                    {
                        new BudgetPost { Amount = 2500, CategoryId = 8, Description = "Barnbidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 23), PostType = BudgetPostType.Income, RecurringId = new Guid("389904d6-c94c-4bd8-8864-27644219f5ad") },
                        new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 25), PostType = BudgetPostType.Expense, RecurringId = new Guid("704d48ad-aa50-44fd-a0ca-8f0b59266f30") },
                        new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("783157fe-c960-43cc-b490-66ac6ca8fad7") },
                        new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 6), PostType = BudgetPostType.Expense, RecurringId = new Guid("30d8676b-de2f-4cf9-90af-18adf034feb5") },
                        new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 5), PostType = BudgetPostType.Income, RecurringId = new Guid("5a36fbcd-47f9-4753-9c16-91d1dbd9eaab") },
                        new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("810421e0-9578-4959-a5e6-0af399ab0cdf") },
                        
                        new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("19cf7680-db76-4047-bb8b-79217df278c1") },
                        new BudgetPost { Amount = 1200, CategoryId = 12, Description = "Veterinärkontroll", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 12), PostType = BudgetPostType.Expense },
                        new BudgetPost { Amount = 800, CategoryId = 16, Description = "Frilansjobb", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 18), PostType = BudgetPostType.Income },
                        new BudgetPost { Amount = 2500, CategoryId = 3, Description = "Bildelar", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 22), PostType = BudgetPostType.Expense },
                        new BudgetPost { Amount = 180, CategoryId = 2, Description = "McDonalds", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 5), PostType = BudgetPostType.Expense },
                        new BudgetPost { Amount = 28500, CategoryId = 14, Description = "Lön", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-1).Year, now.AddMonths(-1).Month, 23), PostType = BudgetPostType.Income }
                    }
                };

                var p3 = new Prognosis
                {
                    Id = Guid.NewGuid(),
                    FromDate = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 1),
                    ToDate = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, DateTime.DaysInMonth(now.AddMonths(-2).Year, now.AddMonths(-2).Month)),
                    Month = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 1).ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture),
                    MonthlyIncome = 30900m,
                    MonthlyExpense = 20930m,
                    TotalSum = 9970m,
                    BudgetPosts = new List<BudgetPost>
                {
                    // RECURRING
                      new BudgetPost { Amount = 2500, CategoryId = 8, Description = "Barnbidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 23), PostType = BudgetPostType.Income, RecurringId = new Guid("389904d6-c94c-4bd8-8864-27644219f5ad") },
                       new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 25), PostType = BudgetPostType.Expense, RecurringId = new Guid("704d48ad-aa50-44fd-a0ca-8f0b59266f30") },
                       new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("783157fe-c960-43cc-b490-66ac6ca8fad7") },
                        new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 6), PostType = BudgetPostType.Expense, RecurringId = new Guid("30d8676b-de2f-4cf9-90af-18adf034feb5") },
                        new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 5), PostType = BudgetPostType.Income, RecurringId = new Guid("5a36fbcd-47f9-4753-9c16-91d1dbd9eaab") },
                        new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("810421e0-9578-4959-a5e6-0af399ab0cdf") },
                        new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("19cf7680-db76-4047-bb8b-79217df278c1") },

                    // NON-RECURRING
                    new BudgetPost { Amount = 200, CategoryId = 13, Description = "Biobesök", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 14), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1500, CategoryId = 16, Description = "Extrajobb helg", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 20), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 180, CategoryId = 2, Description = "McDonalds", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 14), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, CategoryId = 4, Description = "Födelsedagspresent", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 8), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 500, CategoryId = 3, Description = "Tanka bilen", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 8), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 26500, CategoryId = 14, Description = "Lön", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-2).Year, now.AddMonths(-2).Month, 23), PostType = BudgetPostType.Income }
                }
                };

                var p4 = new Prognosis
                {
                    Id = Guid.NewGuid(),
                    FromDate = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 1),
                    ToDate = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, DateTime.DaysInMonth(now.AddMonths(-3).Year, now.AddMonths(-3).Month)),
                    Month = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 1).ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture),
                    MonthlyIncome = 29400m,
                    MonthlyExpense = 26230m,
                    TotalSum = 3170m,
                    BudgetPosts = new List<BudgetPost>
                {
                    // RECURRING
                    new BudgetPost { Amount = 2500, CategoryId = 8, Description = "Barnbidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 23), PostType = BudgetPostType.Income, RecurringId = new Guid("389904d6-c94c-4bd8-8864-27644219f5ad") },
                       new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 25), PostType = BudgetPostType.Expense, RecurringId = new Guid("704d48ad-aa50-44fd-a0ca-8f0b59266f30") },
                       new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("783157fe-c960-43cc-b490-66ac6ca8fad7") },
                        new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 6), PostType = BudgetPostType.Expense, RecurringId = new Guid("30d8676b-de2f-4cf9-90af-18adf034feb5") },
                        new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 5), PostType = BudgetPostType.Income, RecurringId = new Guid("5a36fbcd-47f9-4753-9c16-91d1dbd9eaab") },
                        new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("810421e0-9578-4959-a5e6-0af399ab0cdf") },
                        new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("19cf7680-db76-4047-bb8b-79217df278c1") },

                    // NON-RECURRING
                    new BudgetPost { Amount = 400, CategoryId = 4, Description = "Nya strumpor", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 10), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 500, CategoryId = 13, Description = "Biobesök", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 15), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 480, CategoryId = 2, Description = "Restaurang", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 18), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 5500, CategoryId = 6, Description = "Rörmokaren", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 24), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 28500, CategoryId = 14, Description = "Lön", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-3).Year, now.AddMonths(-3).Month, 23), PostType = BudgetPostType.Income }
                }
                };

                var p5 = new Prognosis
                {
                    Id = Guid.NewGuid(),
                    FromDate = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 1),
                    ToDate = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, DateTime.DaysInMonth(now.AddMonths(-4).Year, now.AddMonths(-4).Month)),
                    Month = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 1).ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture),
                    MonthlyIncome = 30000m,
                    MonthlyExpense = 20200m,
                    TotalSum = 9800m,
                    BudgetPosts = new List<BudgetPost>
                {
                    // RECURRING …
                     new BudgetPost { Amount = 2500, CategoryId = 8, Description = "Barnbidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 23), PostType = BudgetPostType.Income, RecurringId = new Guid("389904d6-c94c-4bd8-8864-27644219f5ad") },
                       new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 25), PostType = BudgetPostType.Expense, RecurringId = new Guid("704d48ad-aa50-44fd-a0ca-8f0b59266f30") },
                       new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("783157fe-c960-43cc-b490-66ac6ca8fad7") },
                        new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 6), PostType = BudgetPostType.Expense, RecurringId = new Guid("30d8676b-de2f-4cf9-90af-18adf034feb5") },
                        new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 5), PostType = BudgetPostType.Income, RecurringId = new Guid("5a36fbcd-47f9-4753-9c16-91d1dbd9eaab") },
                        new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("810421e0-9578-4959-a5e6-0af399ab0cdf") },
                        new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("19cf7680-db76-4047-bb8b-79217df278c1") },

                    // NON-RECURRING
                    new BudgetPost { Amount = 300, CategoryId = 8, Description = "Barnaktiviteter", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 8), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 600, CategoryId = 16, Description = "Säljtjänst", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 17), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 250, CategoryId = 12, Description = "Kattleksak", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 10), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 300, CategoryId = 3, Description = "Tanka bilen", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 8), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 28500, CategoryId = 14, Description = "Lön", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-4).Year, now.AddMonths(-4).Month, 23), PostType = BudgetPostType.Income }
                }
                };

                var p6 = new Prognosis
                {
                    Id = Guid.NewGuid(),
                    FromDate = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 1),
                    ToDate = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, DateTime.DaysInMonth(now.AddMonths(-5).Year, now.AddMonths(-5).Month)),
                    Month = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 1).ToString("MMMM yyyy", System.Globalization.CultureInfo.CurrentCulture),
                    MonthlyIncome = 30000m,
                    MonthlyExpense = 20330m,
                    TotalSum = 9660m,
                    BudgetPosts = new List<BudgetPost>
                {
                    // RECURRING
                    new BudgetPost { Amount = 2500, CategoryId = 8, Description = "Barnbidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 23), PostType = BudgetPostType.Income, RecurringId = new Guid("389904d6-c94c-4bd8-8864-27644219f5ad") },
                       new BudgetPost { Amount = 8500, CategoryId = 6, Description = "Hyra", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 25), PostType = BudgetPostType.Expense, RecurringId = new Guid("704d48ad-aa50-44fd-a0ca-8f0b59266f30") },
                       new BudgetPost { Amount = 1200, CategoryId = 3, Description = "Busskort", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("783157fe-c960-43cc-b490-66ac6ca8fad7") },
                        new BudgetPost { Amount = 750, CategoryId = 3, Description = "Resor till arbete", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 6), PostType = BudgetPostType.Expense, RecurringId = new Guid("30d8676b-de2f-4cf9-90af-18adf034feb5") },
                        new BudgetPost { Amount = 900, CategoryId = 15, Description = "Studiebidrag", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 5), PostType = BudgetPostType.Income, RecurringId = new Guid("5a36fbcd-47f9-4753-9c16-91d1dbd9eaab") },
                        new BudgetPost { Amount = 3200, CategoryId = 2, Description = "Veckohandling", Recurring = Recurring.Weekly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("810421e0-9578-4959-a5e6-0af399ab0cdf") },
                        new BudgetPost { Amount = 1200, CategoryId = 11, Description = "Netflix + Spotify", Recurring = Recurring.Monthly, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 1), PostType = BudgetPostType.Expense, RecurringId = new Guid("19cf7680-db76-4047-bb8b-79217df278c1") },

                    // NON-RECURRING
                    new BudgetPost { Amount = 480, CategoryId = 2, Description = "McDonalds", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 9), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 300, CategoryId = 10, Description = "Kattleksak extra", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 12), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 200, CategoryId = 13, Description = "Biobesök extra", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 6), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 600, CategoryId = 16, Description = "Säljuppdrag", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 21), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 28500, CategoryId = 14, Description = "Lön", Recurring = Recurring.None, Date = new DateTime(now.AddMonths(-5).Year, now.AddMonths(-5).Month, 23), PostType = BudgetPostType.Income }
                }
                };


                var prognoses = new List<Prognosis> { p1, p2, p3, p4, p5, p6 };
                await context.Prognoses.AddRangeAsync(prognoses);
                await context.SaveChangesAsync();
            }
        }
    } 
}