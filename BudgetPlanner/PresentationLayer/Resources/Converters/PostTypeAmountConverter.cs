using BudgetPlanner.DomainLayer.Enums;
using BudgetPlanner.DomainLayer.Models;
using System.Globalization;
using System.Windows.Data;

namespace BudgetPlanner.PresentationLayer.Resources.Converters
{
    public class PostTypeAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           if (value is BudgetPost post)
            {
                string amount = post.Amount.ToString("N0", culture);

                if (post.PostType == BudgetPostType.Expense)
                    return "-" + amount;

                return amount;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
        
    }
}
