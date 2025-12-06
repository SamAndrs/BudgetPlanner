using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<BudgetPost> BudgetPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Prognosis> Prognoses { get; set; }
        public DbSet<RecurringBudgetPostTemplate> RecurringPosts { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // BudgetPost <-- --> Category
            modelBuilder.Entity<BudgetPost>()
                .HasOne(bp => bp.Category)
                .WithMany()
                .HasForeignKey(bp => bp.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prognosis <-- --> BudgetPost
            modelBuilder.Entity<Prognosis>()
                .HasMany(p => p.BudgetPosts)
                .WithOne()
                .HasForeignKey(bp => bp.PrognosisId)
                .OnDelete(DeleteBehavior.Cascade);

            // Set precision/ scale for entities
            modelBuilder.Entity<BudgetPost>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2); // total 18 digits, 2 decimals

            modelBuilder.Entity<Prognosis>()
                .Property(p => p.MonthlyIncome)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Prognosis>()
                .Property(p => p.MonthlyExpense)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Prognosis>()
                .Property(p => p.TotalSum)
                .HasPrecision(18, 2);



            // Seed database
            modelBuilder.Entity<Category>().HasData(
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
                );

            // Seed templates
            modelBuilder.Entity<RecurringBudgetPostTemplate>().HasData(
    new RecurringBudgetPostTemplate
    {
        Id = 1,
        Amount = 28500,
        CategoryId = 14,
        Description = "Lön",
        Recurring = Recurring.Monthly,
        PostType = BudgetPostType.Income,
        RecurringStartDate = new DateTime(2024, 1, 23)
    },
                new RecurringBudgetPostTemplate
                {
                    Id = 2,
                    Amount = 8500,
                    CategoryId = 6,
                    Description = "Hyra",
                    Recurring = Recurring.Monthly,
                    PostType = BudgetPostType.Expense,
                    RecurringStartDate = new DateTime(2024, 1, 25)
                },
                new RecurringBudgetPostTemplate
                {
                    Id = 3,
                    Amount = 1200,
                    CategoryId = 3,
                    Description = "Busskort",
                    Recurring = Recurring.Monthly,
                    PostType = BudgetPostType.Expense,
                    RecurringStartDate = new DateTime(2024, 1, 1)
                },
                new RecurringBudgetPostTemplate
                {
                    Id = 4,
                    Amount = 750,
                    CategoryId = 3,
                    Description = "Resor till arbete",
                    Recurring = Recurring.Monthly,
                    PostType = BudgetPostType.Expense,
                    RecurringStartDate = new DateTime(2024, 1, 6)
                },
                new RecurringBudgetPostTemplate
                {
                    Id = 5,
                    Amount = 900,
                    CategoryId = 15,
                    Description = "Studiebidrag",
                    Recurring = Recurring.Monthly,
                    PostType = BudgetPostType.Income,
                    RecurringStartDate = new DateTime(2024, 1, 5)
                },
                new RecurringBudgetPostTemplate
                {
                    Id = 6,
                    Amount = 3200,
                    CategoryId = 2,
                    Description = "Veckohandling",
                    Recurring = Recurring.Weekly,
                    PostType = BudgetPostType.Expense,
                    RecurringStartDate = new DateTime(2024, 1, 1)
                },
                new RecurringBudgetPostTemplate
                {
                    Id = 7,
                    Amount = 1200,
                    CategoryId = 11,
                    Description = "Netflix + Spotify",
                    Recurring = Recurring.Monthly,
                    PostType = BudgetPostType.Expense,
                    RecurringStartDate = new DateTime(2024, 1, 1)
                },
                new RecurringBudgetPostTemplate
                {
                    Id = 8,
                    Amount = 4500,
                    CategoryId = 3,
                    Description = "Bilskatt",
                    Recurring = Recurring.Yearly,
                    PostType = BudgetPostType.Expense,
                    RecurringStartDate = new DateTime(2024, 3, 12)
                }
            );
        }
    }
}
