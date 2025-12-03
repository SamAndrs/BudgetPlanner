using System.Windows;
using System.Windows.Controls;

namespace BudgetPlanner.PresentationLayer.Resources.BudgetPosts
{

    public partial class CategoryBarChart : UserControl
    {
        public CategoryBarChart()
        {
            InitializeComponent();
        }

        public Dictionary<string, int> Data
        {
            get => (Dictionary<string, int>)GetValue(Dataproperty);
            set => SetValue(Dataproperty, value);
        }

        public static readonly DependencyProperty Dataproperty =
            DependencyProperty.Register(
                "Data",
                typeof(Dictionary<string, int>),
                typeof(CategoryBarChart),
                new PropertyMetadata(null, OnDataChanged));



        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CategoryBarChart)d;
            control.Redraw();
        }

        private void Redraw()
        {
            if (Data == null || Data.Count == 0)
            {
                Plot.Plot.Clear();
                Plot.Refresh();
                return;
            }

            var plt = Plot.Plot;
            plt.Clear();

            var categories = Data.Keys.ToArray();
            var values = Data.Values.Select(v => (double)v).ToArray();
            int maxValue = 0;

            var bars = new List<ScottPlot.Bar>();

            for (int i = 0; i < categories.Length; i++)
            {
                bars.Add(new ScottPlot.Bar()
                {
                    Position = i,
                    Value = values[i],
                    FillColor = GetTresholdColor(values[i]),
                });

                maxValue += (int)bars[i].Value;
            }


            var barPlot = plt.Add.Bars(bars);
            barPlot.Horizontal = true;

            // Axis setup
            plt.Axes.SetLimitsX(0, maxValue * 1.1); // 10% marginal
            plt.Axes.Left.TickGenerator = new ScottPlot.TickGenerators.NumericManual(
                            Enumerable.Range(0, categories.Length)
                            .Select(i => new ScottPlot.Tick(i, categories[i]))
                            .ToArray()
            );


            // Styling
            plt.Axes.Color(SetColor("LabelText"));
            plt.FigureBackground.Color = ScottPlot.Colors.Transparent;
            plt.DataBackground.Color = ScottPlot.Colors.Transparent;

            Plot.Refresh();
        }


        private ScottPlot.Color GetTresholdColor(double value)
        {
            if (value <= 2)
                return ScottPlot.Color.FromHex("#4CAF50");     // Green 500

            if (value <= 5)
                return ScottPlot.Color.FromHex("#FFC107");     // Amber 500

            return ScottPlot.Color.FromHex("#F44336");          // Red 500
        }


        private ScottPlot.Color SetColor(string color)
        {
            switch (color)
            {
                case ("LabelText"):
                    return ScottPlot.Color.FromHex("#8c8c8c");

                case ("GridLine"):
                    return ScottPlot.Color.FromHex("#242424");

                default:
                    return ScottPlot.Color.FromHex("#ffffff");
            }
        }
    }
}
