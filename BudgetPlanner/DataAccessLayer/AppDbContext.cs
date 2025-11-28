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
        }
    }
}
