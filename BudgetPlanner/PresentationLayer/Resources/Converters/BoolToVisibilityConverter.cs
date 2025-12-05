using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BudgetPlanner.PresentationLayer.Resources.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = value is bool b && b;

            if(parameter is string p && p.ToLower() == "invert")
                visible = !visible;

            if (Invert)
                visible = !visible;

            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotImplementedException();
    }
}
