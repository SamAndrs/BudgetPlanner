using System.ComponentModel;

namespace BudgetPlanner.DomainLayer.Enums
{
    public enum Recurring
    {
        [Description("Inget")]
        None,

        [Description("Dagligen")]
        Daily,

        [Description("Veckovis")]
        Weekly,

        [Description("Månadsvis")]
        Monthly,

        [Description("Årligen")]
        Yearly,
    }
}
