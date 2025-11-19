using BudgetPlanner.DomainLayer.Enums;

namespace BudgetPlanner.DomainLayer.Models
{
    public class BudgetPost
    {
        public int Id { get; set; }

        public double Amount { get; set; } = 0;

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public Recurring Reccuring { get; set; } = Recurring.None;

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public TransactionType TransactionType { get; set; } = TransactionType.Expense;
    }
}
