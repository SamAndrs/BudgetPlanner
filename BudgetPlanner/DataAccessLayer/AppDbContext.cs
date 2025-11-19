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
                new Category { Id = 1,Name = "Food" },
                new Category { Id = 2, Name = "Transport" },
                new Category { Id = 3, Name = "Clothing" },
                new Category { Id = 4, Name = "Taxes" },
                new Category { Id = 5, Name = "House" },
                new Category { Id = 6, Name = "Hobbies" },
                new Category { Id = 7, Name = "Kids" },
                new Category { Id = 8, Name = "TV" },
                new Category { Id = 9, Name = "SaaS" },
                new Category { Id = 10, Name = "Subscriptions" }
                );
        }
    }
}
