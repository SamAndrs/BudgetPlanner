using BudgetPlanner.DomainLayer.Models;
using ScottPlot;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;

namespace BudgetPlanner.PresentationLayer.Resources.PrognosisView
{

    public partial class PrognosisChart : UserControl
    {
        public PrognosisChart()
        {
            InitializeComponent();
        }

        public IEnumerable<Prognosis> Data
        {
            get => (IEnumerable<Prognosis>)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(
                nameof(Data),
                typeof(IEnumerable<Prognosis>),
                typeof(PrognosisChart),
                new PropertyMetadata(null, OnDataChanged));

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = d as PrognosisChart;
            chart?.RenderChart();
        }

        private void RenderChart()
        {
            if (Data == null || !Data.Any() || PlotControl == null)
                return;

            var plt = PlotControl.Plot;
            plt.Clear();

            // Sortera datan i kronologisk ordning
            var months = Data.OrderBy(p => p.FromDate).ToList();

            double[] incomes = months.Select(m => (double)m.TotalIncome).ToArray();
            double[] expenses = months.Select(m => (double)m.TotalExpenses).ToArray();

            string[] labels = months
                .Select(m => m.FromDate.ToString("MMM"))
                .ToArray();

            double[] xs = Enumerable.Range(0, labels.Length).Select(i => (double)i).ToArray();

            var bars = new List<Bar>();

            // Stapelbredd
            double barWidth = 0.2;

            for (int i = 0; i < months.Count; i++)
            {
                var month = months[i];
                double pos = i;

                // Skapa bars
                bars.Add(new Bar
                {
                    Position = pos - 0.2,
                    Value = incomes[i],
                    FillColor = SetColor("Income"),
                    Size = barWidth

                    
                });

                bars.Add(new Bar
                {
                    Position = pos + 0.2,
                    Value = -expenses[i],
                    FillColor = SetColor("Expense"),
                    Size = barWidth
                });
            }

            /* Income bars → GRÖN
            plt.AddBar(
                values: incomes,
                positions: xs.Select(x => x - barWidth / 2).ToArray(),
                color: Colors.Green.WithOpacity(0.85),
                fill: true,
                barWidth: barWidth
            );

            // Expense bars → RÖD
            plt.AddBar(
                values: expenses.Select(x => -x).ToArray(),   // negativa för att peka nedåt
                positions: xs.Select(x => x + barWidth / 2).ToArray(),
                color: Colors.Red.WithOpacity(0.85),
                fill: true,
                barWidth: barWidth
            );*/

            plt.Add.Bars(bars);

            // Y-axel titel
            plt.YLabel("Belopp (kr)");

            // X-axel
            //plt.XAxis.TickGenerator = new ScottPlot.TickGenerators.Category(labels);


            // Grid & stil
            //plt.Grid(enable: true, lineStyle: LineStyle.Dot, color: ScottPlot.Colors.Gray.WithOpacity(0.3));
            plt.Axes.Color(SetColor("LabelText"));
            plt.FigureBackground.Color = Colors.Transparent;
            plt.DataBackground.Color = Colors.Transparent;

            plt.Title("Inkomster och utgifter per månad");

            PlotControl.Refresh();
        }

        private Color SetColor(string color)
        {
            switch (color)
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

    // Små extension helpers
    public static class ColorExtensions
    {
        public static Color WithOpacity(this Color c, double opacity)
        {
            return new Color(c.R, c.G, c.B, (byte)(opacity * 255));
        }
    }


}

