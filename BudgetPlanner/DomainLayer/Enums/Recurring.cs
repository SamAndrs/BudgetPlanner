using System.ComponentModel;

namespace BudgetPlanner.DomainLayer.Enums
{
    public enum Recurring
    {
        [Description("Tillfälligt")]
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
