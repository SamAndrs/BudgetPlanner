using BudgetPlanner.DomainLayer.Enums;

namespace BudgetPlanner.DomainLayer.Models
{
    public class RecurringBudgetPostTemplate
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string Description { get; set; } = string.Empty;

        public Recurring Recurring { get; set; }

        public BudgetPostType PostType { get; set; }

        public DateTime RecurringStartDate { get; set; }
    }
}
