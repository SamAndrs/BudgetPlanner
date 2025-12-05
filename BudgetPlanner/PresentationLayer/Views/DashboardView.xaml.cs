using BudgetPlanner.PresentationLayer.ViewModels;
using ScottPlot;
using System.Windows.Controls;

namespace BudgetPlanner.PresentationLayer.Views
{
    public partial class DashboardView : UserControl
    {

        public DashboardView()
        {
            InitializeComponent();
            Loaded += DashboardView_Loaded;

        }

        private void DashboardView_Loaded(object sender, System.Windows.RoutedEventArgs e)
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

            if (vm.ExpenseValues.Count == 0)
                return;

            int count = vm.ExpenseValues.Count;
            double total = vm.ExpenseValues.Sum();

            List<PieSlice> slices = new(count);

            // Calculate percentages and set colors
            for (int i = 0; i < count; i++)
            {
                double percentage = (vm.ExpenseValues[i] / total) * 100;
                var color = Palette.Default.GetColor(i);

                slices.Add(new PieSlice()
                {
                    Value = vm.ExpenseValues[i],
                    Label = $"{vm.ExpenseLabels[i]}\n{percentage:0.0}%",
                    FillColor = color,
                    LabelFontColor = color
                });
            }

            // Create chart
            var pie = plt.Add.Pie(slices);

            // Styling
            pie.ExplodeFraction = 0.1;
            pie.DonutFraction = 0.4;
            pie.SliceLabelDistance = 1.2;

            plt.Axes.Frameless();
            plt.Axes.SetLimits(-1.5, 1.5, -1.5, 1.5);

            plt.FigureBackground.Color = Colors.Transparent;

            PieChartPlot.Refresh();
        }

       
        private void DrawDailyBarChart(DashboardViewVM vm)
        {
            var plt = DailyBarChartPlot.Plot;
            plt.Clear();

            var activeDays = vm.ActiveDays;
            int nrDays = activeDays.Count;

            var bars = new List<Bar>();
            var ticks = new List<Tick>();

            for (int i = 0; i < nrDays; i++)
            {
                var day = activeDays[i];
                double pos = i;

                // Skapa bars
                bars.Add(new Bar
                {
                    Position = pos - 0.1,
                    Value = day.TotalIncome,
                    FillColor = SetColor("Income"),
                    Size = 0.2
                });

                bars.Add(new Bar
                {
                    Position = pos + 0.1,
                    Value = day.TotalExpense,
                    FillColor = SetColor("Expense"),
                    Size = 0.2
                });

                // Skapa X‑axel tick
                ticks.Add(new Tick(pos, day.DayNumber.ToString()));
            }

            // Lägg till alla bars på en gång
            plt.Add.Bars(bars);

            // Axis setup  (Sätt Y- och X-axlar från 0 till maxvärde + lite marginal)
            if(activeDays.Any())
            {
                double maxValue = activeDays.Max(d => Math.Max(d.TotalIncome, d.TotalExpense));
                plt.Axes.SetLimitsY(0, maxValue * 1.1); // 10% marginal

                // X‑axel
                plt.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(ticks.ToArray());
                plt.Axes.SetLimitsX(0, ticks.Count * 1.1); // 10% marginal
            }
            

            // Styling
            plt.Axes.Color(SetColor("LabelText"));
            plt.FigureBackground.Color = Colors.Transparent;
            plt.DataBackground.Color = Colors.Transparent;

            DailyBarChartPlot.Refresh();
        }


      
        private Color SetColor(string color)
        {
            switch(color)
            {
                case ("Income"):
                    return Color.FromHex("#39dc9c");

                case ("Expense"):
                    return Color.FromHex("#d15356");

                case ("LabelText"):
                    return Color.FromHex("#8c8c8c");

                case ("GridLine"):
                    return Color.FromHex("#242424");

                default:
                    return Color.FromHex("#ffffff");
            }
        }
    }


}
