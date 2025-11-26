using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlanner.DomainLayer.Models
{
    public class Prognosis
    {
        public int Id { get; set; }

        public virtual List<BudgetPost> BudgetPosts { get; set; } = new();

        public decimal TotalIncome { get; set; } = 0;

        public decimal TotalExpenses { get; set; } = 0;

        public decimal TotalSum { get; set; } = 0;

        public DateTime FromDate { get; set; } = DateTime.Now;

        public DateTime ToDate { get; set; }

        public string Month { get; set; } = string.Empty;
    }
}
