using System.ComponentModel;

namespace BudgetPlanner.DomainLayer.Enums
{
    public enum BudgetPostType
    {
        [Description("Inkomst")]
        Income,

        [Description("Utgift")]
        Expense,
    }
}
