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
            var Categories = context.Categories.ToList();

            if (!context.BudgetPosts.Any())
            {
                var newPosts = new List<BudgetPost>
                {
                    // ===== Månad 5: Fyra månader bakåt =====
                    new BudgetPost { Amount = 28500, Category = Categories[13], CategoryId = 14, Description = "Lön", Date = new DateTime(now.Year, now.Month - 4, 23), Recurring = Recurring.Monthly, RecurringGroupID = Guid.NewGuid().ToString(), PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 8500, Category = Categories[4], CategoryId = 6, Description = "Hyra", Date = new DateTime(now.Year, now.Month - 4, 25), Recurring = Recurring.Monthly, RecurringGroupID = Guid.NewGuid().ToString(), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, Category = Categories[2], CategoryId= 3, Description = "Busskort", Date = new DateTime(now.Year, now.Month-4, 1), Recurring= Recurring.Monthly, RecurringGroupID = Guid.NewGuid().ToString(), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 750, Category = Categories[2], CategoryId = 3, Description = "Resor till arbete", Date = new DateTime(now.Year, now.Month - 4, 6), Recurring = Recurring.Monthly, RecurringGroupID = Guid.NewGuid().ToString(), PostType = BudgetPostType.Expense },
                    new BudgetPost {  Amount = 900, Category = Categories[14], CategoryId = 15, Description = "Studiebidrag", Date = new DateTime(now.Year, now.Month - 4, 5), Recurring = Recurring.Monthly, RecurringGroupID = Guid.NewGuid().ToString(), PostType = BudgetPostType.Income },
                    new BudgetPost {  Amount = 3200, Category = Categories[1], CategoryId = 2, Description = "Veckohandling", Date = new DateTime(now.Year, now.Month-4, 1), Recurring = Recurring.Weekly, RecurringGroupID = Guid.NewGuid().ToString(), PostType = BudgetPostType.Expense },
                    new BudgetPost {  Amount = 1200, Category = Categories[9], CategoryId = 11, Description = "Netflix + Spotify", Date = new DateTime(now.Year, now.Month - 4, 1), Recurring = Recurring.Monthly, RecurringGroupID = Guid.NewGuid().ToString(), PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 4500, Category = Categories[2], CategoryId= 3, Description = "Bilskatt", Date = new DateTime(now.Year, now.Month-3, 12), Recurring= Recurring.Yearly, RecurringGroupID = Guid.NewGuid().ToString(), PostType = BudgetPostType.Expense },

                    new BudgetPost { Amount = 300, Category = Categories[6], CategoryId = 8, Description = "Barnaktiviteter", Date = new DateTime(now.Year, now.Month - 4, 8), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 600, Category = Categories[14], CategoryId = 16, Description = "Säljtjänst", Date = new DateTime(now.Year, now.Month - 4, 17), Recurring = Recurring.None, PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 250, Category = Categories[10], CategoryId = 12, Description = "Kattleksak", Date = new DateTime(now.Year, now.Month-4, 10), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 300, Category = Categories[2], CategoryId= 3, Description = "Tanka bilen", Date = new DateTime(now.Year, now.Month-4, 8), Recurring= Recurring.None, PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 480, Category = Categories[1], CategoryId = 2, Description = "McDonalds", Date = new DateTime(now.Year, now.Month - 2, 8), Recurring = Recurring.None, PostType = BudgetPostType.Expense },

                    
                    // ===== Månad 4: Tre månader bakåt =====
                    new BudgetPost { Amount = 400, Category = Categories[2], CategoryId = 4, Description = "Nya strumpor", Date = new DateTime(now.Year, now.Month - 3, 10), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 500, Category = Categories[12], CategoryId= Categories[13].Id, Description = "Biobesök", Date = new DateTime(now.Year, now.Month-3, 15), Recurring= Recurring.None, PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 480, Category = Categories[1], CategoryId = 2, Description = "Restaurang", Date = new DateTime(now.Year, now.Month-3, 18), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 5500, Category = Categories[4], CategoryId = 6, Description = "Rörmokaren", Date = new DateTime(now.Year, now.Month - 3, 24), Recurring = Recurring.None, PostType = BudgetPostType.Expense },

                    // ===== Månad 3: Två månader bakåt =====
                    new BudgetPost { Amount = 200, Category = Categories[11], CategoryId = 13, Description = "Biobesök", Date = new DateTime(now.Year, now.Month - 2, 14), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1500, Category = Categories[15], CategoryId = 16, Description = "Extrajobb helg", Date = new DateTime(now.Year, now.Month - 2, 20), Recurring = Recurring.None, PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 180, Category = Categories[1], CategoryId = 2, Description = "McDonalds", Date = new DateTime(now.Year, now.Month-2, 14), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, Category = Categories[2], CategoryId = 4, Description = "Födelsedagspresent", Date = new DateTime(now.Year, now.Month - 2, 8), Recurring = Recurring.None, PostType = BudgetPostType.Expense },

                     // ===== Månad 2: En månad bakåt  =====
                    new BudgetPost { Amount = 1200, Category = Categories[10], CategoryId = 12, Description = "Veterinärkontroll", Date = new DateTime(now.Year, now.Month-1, 12), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 800, Category = Categories[14], CategoryId = 16, Description = "Frilansjobb", Date = new DateTime(now.Year, now.Month-1, 18), Recurring = Recurring.None, PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 2500, Category = Categories[2], CategoryId= 3, Description = "Bildelar", Date = new DateTime(now.Year, now.Month-1, 22), Recurring= Recurring.None, PostType = BudgetPostType.Expense },


                    // ==== Nuvarande månad ====
                    new BudgetPost { Amount = 500, Category = Categories[12], CategoryId= Categories[13].Id, Description = "Biobesök", Date = new DateTime(now.Year, now.Month, 15), Recurring= Recurring.None, PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1200, Category = Categories[15], CategoryId= Categories[15].Id, Description = "Bonus", Date = new DateTime(now.Year, now.Month, 15), Recurring= Recurring.None, PostType = BudgetPostType.Income },
                    new BudgetPost { Amount = 480, Category = Categories[1], CategoryId = 2, Description = "Restaurang", Date = new DateTime(now.Year, now.Month, 8), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 180, Category = Categories[1], CategoryId = 2, Description = "McDonalds", Date = new DateTime(now.Year, now.Month, 26), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                    new BudgetPost { Amount = 1500, Category = Categories[7], CategoryId = 8, Description = "Julklappar - barnen", Date = new DateTime(now.Year, now.Month, 6), Recurring = Recurring.None, PostType = BudgetPostType.Expense },
                };
                
                context.BudgetPosts.AddRange(newPosts);
            }
            await context.SaveChangesAsync();
        }
    }
}
