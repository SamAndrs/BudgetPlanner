using BudgetPlanner.DataAccessLayer;
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
    }
}
