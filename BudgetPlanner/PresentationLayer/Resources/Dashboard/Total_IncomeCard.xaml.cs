using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BudgetPlanner.PresentationLayer.Resources.Dashboard
{
    public partial class Total_IncomeCard : UserControl
    {
        public Total_IncomeCard()
        {
            InitializeComponent();
            this.DataContextChanged += (s, e) => RootPanel.DataContext = DataContext;
        }
    }
}
