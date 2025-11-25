using BudgetPlanner.DomainLayer.Services;
using BudgetPlanner.PresentationLayer.ViewModels;
using System.Windows;

namespace BudgetPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

        }
    }
}