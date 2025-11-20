using BudgetPlanner.PresentationLayer.ViewModels;
using System.Windows.Controls;

namespace BudgetPlanner.PresentationLayer.Views
{
    
    public partial class BudgetPostTestView : UserControl
    {
        public BudgetPostTestView()
        {
            InitializeComponent();
            DataContext = new BudgetPostTestVM();
        }
    }
}
