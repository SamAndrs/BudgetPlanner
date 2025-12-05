using BudgetPlanner.DataAccessLayer;
using BudgetPlanner.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.DomainLayer.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _db;

        public CategoryService(AppDbContext db)
        {
            _db = db;
        }

        public List<Category> GetAllCategories() => _db.Categories.OrderBy(c => c.Name).ToList();
    }
}
