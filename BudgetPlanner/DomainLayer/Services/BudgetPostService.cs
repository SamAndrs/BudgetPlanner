using BudgetPlanner.Data;
using BudgetPlanner.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner.DomainLayer.Services
{
    public class BudgetPostService
    {
        private readonly AppDbContext _db;

        public BudgetPostService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<BudgetPost>> GetAllPostsAsync() => await _db.BudgetPosts.ToListAsync();
    }
}
