using BudgetPlanner.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<BudgetPost> BudgetPosts { get; set; }
        public DbSet<Category> Categories { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
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

            // Seed database
            modelBuilder.Entity<Category>().HasData(
                new Category { Name = "Food" },
                new Category { Name = "Transport" },
                new Category { Name = "Clothing" },
                new Category { Name = "Taxes" },
                new Category { Name = "Salary" },
                new Category { Name = "House" },
                new Category { Name = "Hobbies" },
                new Category { Name = "Kids" },
                new Category { Name = "TV" },
                new Category { Name = "SaaS" },
                new Category {  Name = "Subscriptions" },
                new Category { Name = "Allowance" },
                new Category { Name = "Undefined" }
                );
        }
    }
}
