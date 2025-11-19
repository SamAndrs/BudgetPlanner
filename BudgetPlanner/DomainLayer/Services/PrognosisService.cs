using BudgetPlanner.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.DomainLayer.Services
{
    public class PrognosisService
    {
        private readonly AppDbContext _db;

        public PrognosisService(AppDbContext db)
        {
            _db = db;
        }
    }
}
