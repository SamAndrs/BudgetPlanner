using BudgetPlanner.DomainLayer.Services;
using BudgetPlanner.PresentationLayer.ViewModels;
using System.Windows;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace BudgetPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            // Watch for system theme changes (WPF UI)
            SystemThemeWatcher.Watch(this);

            InitializeComponent();
            DataContext = new MainViewModel();

        }
    }
}