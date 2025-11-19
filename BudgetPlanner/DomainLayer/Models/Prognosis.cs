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

        public decimal TotalIncome { get; set; }

        public decimal TotalExpenses { get; set; }

        public decimal TotalSum { get; set; }

        public DateTime Date { get; set; }
    }
}
