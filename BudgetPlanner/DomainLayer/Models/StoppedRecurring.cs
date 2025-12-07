namespace BudgetPlanner.DomainLayer.Models
{
    // List of BudgetPosts no longer to be recurring
    public class StoppedRecurring
    {
        public Guid Id { get; set; }

        public Guid RecurringId { get; set; }

        public DateTime StoppedAt { get; set; }
    }
}
