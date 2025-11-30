using BudgetPlanner.DomainLayer.Enums;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BudgetPlanner.PresentationLayer.Resources.Converters
{
    public class PostTypeToBGColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BudgetPostType type)
            {
                return type == BudgetPostType.Income
                    ? Application.Current.Resources["IncomeBackground"]
                    : Application.Current.Resources["ExpenseBackground"];
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
    }
}
