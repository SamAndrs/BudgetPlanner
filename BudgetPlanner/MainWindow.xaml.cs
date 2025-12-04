using BudgetPlanner.DomainLayer.Services;
using BudgetPlanner.PresentationLayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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
            //DataContext = new MainViewModel();
            DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void btnWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }
    }
}