using BudgetPlanner.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<BudgetPost> BudgetPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Prognosis> Prognoses { get; set; }

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

            // Set precision/ scale for entities
            modelBuilder.Entity<BudgetPost>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2); // total 18 digits, 2 decimals

            modelBuilder.Entity<Prognosis>()
                .Property(p => p.TotalIncome)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Prognosis>()
                .Property(p => p.TotalExpenses)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Prognosis>()
                .Property(p => p.TotalSum)
                .HasPrecision(18, 2);



            // Seed database
            modelBuilder.Entity<Category>().HasData(
                // Expenses
                new Category { Id = 1, Name = "Food" },
                new Category { Id = 2, Name = "Transport" },
                new Category { Id = 3, Name = "Clothing" },
                new Category { Id = 4, Name = "Taxes" },
                new Category { Id = 5, Name = "House" },
                new Category { Id = 6, Name = "Hobbies" },
                new Category { Id = 7, Name = "Kids" },
                new Category { Id = 8, Name = "TV" },
                new Category { Id = 9, Name = "SaaS" },
                new Category { Id = 10, Name = "Subscriptions" },
                
                // Income
                new Category { Id = 11, Name = "Salary" },
                new Category { Id = 12, Name = "Allowance" },
                new Category { Id = 13, Name = "ExtraIncome" },
                new Category { Id = 14, Name = "Undefined" }
                );
        }
    }
}
