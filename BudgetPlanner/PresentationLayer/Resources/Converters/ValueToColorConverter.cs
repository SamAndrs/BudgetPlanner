using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BudgetPlanner.PresentationLayer.Resources.Converters
{
    public class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val)
            {
                if (val > 0)
                    return Application.Current.Resources["IncomeGreen"];

                if (val < 0)
                    return Application.Current.Resources["ExpenseRed"];
            }

            // Zero or default
            return Application.Current.Resources["PrimaryForeground"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
