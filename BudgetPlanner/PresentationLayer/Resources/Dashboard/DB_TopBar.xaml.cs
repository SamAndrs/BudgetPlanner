using System.Windows;
using System.Windows.Controls;

namespace BudgetPlanner.PresentationLayer.Resources.Dashboard
{

    public partial class DB_TopBar : UserControl
    {
        public DB_TopBar()
        {
            InitializeComponent();
            // Gör att containerns DataContext skickas vidare till dess innehåll
            this.DataContextChanged += (s, e) => RootPanel.DataContext = DataContext;
        }

    }
}
