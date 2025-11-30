using BudgetPlanner.PresentationLayer.ViewModels;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Xaml.Schema;

namespace BudgetPlanner.PresentationLayer.Views
{
    public partial class DashboardView : UserControl
    {

        public DashboardView()
        {
            InitializeComponent();
            Loaded += DazhboardView_Loaded;

        }

        private void DazhboardView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = DataContext as DashboardViewVM;
            if (vm == null) return;

            DrawDailyBarChart(vm);
            DrawPieChart(vm);
        }

        private void DrawPieChart(DashboardViewVM vm)
        {
            var plt = PieChartPlot.Plot;
            plt.Clear();

            // Set background to transparent
            plt.Style(figureBackground: System.Drawing.Color.Transparent,
                dataBackground: System.Drawing.Color.Transparent);


            // Create piechart
            var pie = plt.AddPie(vm.ExpenseValues.ToArray());

            pie.SliceLabels = vm.ExpenseLabels.ToArray();
            //pie.ShowLabels = true;

            //pie.ShowValues = false;
            pie.ShowPercentages = true;
            pie.Explode = true;
            pie.DonutSize = .5;

            plt.Legend(location: ScottPlot.Alignment.LowerRight);
            PieChartPlot.Refresh();
        }

        private void DrawDailyBarChart(DashboardViewVM vm)
        {
            // Set colors
            var textColor = ToColor((System.Windows.Media.Color)Application.Current.Resources["SecondaryForegroundColor"]);
            var gridLineColor = ToColor((System.Windows.Media.Color)Application.Current.Resources["PrimaryBackgroundColor"]);

            var incomeBrush = (System.Windows.Media.SolidColorBrush)Application.Current.Resources["IncomeGreen"];
            var expenseBrush = (System.Windows.Media.SolidColorBrush)Application.Current.Resources["ExpenseRed"];
            
            var incomeColor = ToColor(incomeBrush.Color);
            var expenseColor = ToColor(expenseBrush.Color);

            var plt = DailyBarChartPlot.Plot;
            plt.Clear();

            var activeDays = vm.ActiveDays;
            int nrDays = activeDays.Count;

            double[] xs = new double[nrDays];
            double[] incomeValues = new double[nrDays];
            double[] expensesValues = new double[nrDays];
            string[] labels = new string[nrDays];

            for (int i = 0; i < nrDays; i++)
            {
                xs[i] = i;
                incomeValues[i] = activeDays[i].TotalIncome;
                expensesValues[i] = activeDays[i].TotalExpense;
                labels[i] = activeDays[i].DayNumber.ToString();
            }

            // Add bars
            var incomeBars = plt.AddBar(incomeValues, xs);
            incomeBars.FillColor = incomeColor;
            incomeBars.BarWidth = 0.2;
            incomeBars.PositionOffset = -0.2;

            var expenseBars = plt.AddBar(expensesValues, xs);
            expenseBars.FillColor = expenseColor;
            expenseBars.BarWidth = 0.2;
            expenseBars.PositionOffset = +0.2;

            // X-Axis
            plt.XTicks(xs, labels);
            plt.XLabel("Dag i månaden");
            plt.YLabel("Belopp");

            // Legend
            plt.Legend();

            // Styling
            plt.XAxis.Color(textColor);
            plt.YAxis.Color(textColor);

            // Set major grid line color
            plt.XAxis.MajorGrid(true, gridLineColor);
            plt.YAxis.MajorGrid(true, gridLineColor);

            // Set background to transparent
            plt.Style(figureBackground: System.Drawing.Color.Transparent,
                dataBackground: System.Drawing.Color.Transparent);

            DailyBarChartPlot.Refresh();
        }

        private Color ToColor(System.Windows.Media.Color mediaColor)
        {
            return System.Drawing.Color.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);
        }

        
    }


}
