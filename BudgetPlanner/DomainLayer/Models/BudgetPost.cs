using BudgetPlanner.DomainLayer.Enums;

namespace BudgetPlanner.DomainLayer.Models
{
    public class BudgetPost
    {
        public int Id { get; set; }

        public double Amount { get; set; } = 0;

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string Description { get; set; } = string.Empty;

        public Recurring Recurring { get; set; } = Recurring.None;

        public string? RecurringGroupID { get; set; }

        public DateTime? Date { get; set; } = DateTime.UtcNow;

        public BudgetPostType PostType { get; set; }
    }
}
