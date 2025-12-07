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
        public DbSet<StoppedRecurring> StoppedRecurringPosts { get; set; }

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
                    RecurringId = new Guid("389904d6-c94c-4bd8-8864-27644219f5ad"),
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
                    RecurringId = new Guid("704d48ad-aa50-44fd-a0ca-8f0b59266f30"),
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
                    RecurringId = new Guid("783157fe-c960-43cc-b490-66ac6ca8fad7"),
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
                    RecurringId = new Guid("30d8676b-de2f-4cf9-90af-18adf034feb5"),
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
                    RecurringId = new Guid("5a36fbcd-47f9-4753-9c16-91d1dbd9eaab"),
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
                    RecurringId = new Guid("810421e0-9578-4959-a5e6-0af399ab0cdf"),
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
                    RecurringId = new Guid("19cf7680-db76-4047-bb8b-79217df278c1"),
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
                    RecurringId = new Guid("67f4ec7c-13a9-4973-a884-80b009b9f3b0"),
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
