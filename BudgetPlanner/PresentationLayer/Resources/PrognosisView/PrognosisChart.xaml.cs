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

            string[] xLabels = months
                .Select(m => m.FromDate.ToString("MMM"))
                .ToArray();

           

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
                    Position = pos - 0.1,
                    Value = incomes[i],
                    FillColor = SetColor("Income"),
                    Size = barWidth

                    
                });

                bars.Add(new Bar
                {
                    Position = pos + 0.1,
                    Value = expenses[i],
                    FillColor = SetColor("Expense"),
                    Size = barWidth
                });
            }

            // Build chart
            plt.Add.Bars(bars);

            // Y-axis title
            plt.YLabel("Belopp (kr)");
            var ymaxValue = GetMaxValue(incomes, expenses); // Get highest of either income/ expense
            
            //ScottPlot.TickGenerators.NumericAutomatic tickGenY = new();
            //tickGenY.TargetTickCount = 3;
            //plt.Axes.Left.TickGenerator = tickGenY;
            plt.Axes.SetLimitsY(0, ymaxValue * 1.1);


            // X-axis
            plt.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
                            Enumerable.Range(0, xLabels.Length)
                            .Select(i => new Tick(i, xLabels[i]))
                            .ToArray()
            );

            // Grid & stil
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


        private int GetMaxValue(double[] incomes, double[] expenses)
        {
            var highestIncome = incomes.Max();

            var highestExpenses = expenses.Max();

            return (int)Math.Max(highestIncome, highestExpenses);
        }
    }
}

