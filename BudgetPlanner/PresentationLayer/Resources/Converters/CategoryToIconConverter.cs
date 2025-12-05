using BudgetPlanner.DomainLayer.Models;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BudgetPlanner.PresentationLayer.Resources.Converters
{
    public class CategoryToIconConverter : IValueConverter
    {
        public record CategoryIcon(string Emoji, Brush Color);

        // Mappa Category.Name → Icon (Emoji)
        private static readonly Dictionary<string, CategoryIcon> _iconMap = new()
        {
            { "Alla", new CategoryIcon("🗂️", Brushes.Yellow) },
            { "Mat", new CategoryIcon("🍎", Brushes.OrangeRed) },
            { "Transport", new CategoryIcon("🚗",Brushes.Red) },
            { "Kläder", new CategoryIcon("👕", Brushes.White) },
            { "Skatt", new CategoryIcon("🏛️", Brushes.Gray) },
            { "Hem", new CategoryIcon("🏠", Brushes.Brown) },
            { "Hobby", new CategoryIcon("🎨", Brushes.Magenta) },
            { "Barn", new CategoryIcon("🧸", Brushes.Brown) },
            { "TV", new CategoryIcon("📺", Brushes.DarkSlateGray) },
            { "SaaS", new CategoryIcon("☁️", Brushes.SkyBlue) },
            { "Prenumerationer", new CategoryIcon("💳", Brushes.DarkSlateBlue) },
            { "Husdjur", new CategoryIcon("🐶", Brushes.SaddleBrown) },
            { "Underhållning", new CategoryIcon("🎉", Brushes.Gold) },

            { "Lön", new CategoryIcon("💰", Brushes.DarkGreen) },
            { "Bidrag", new CategoryIcon("🪙", Brushes.Gold) },
            { "Extrainkomst", new CategoryIcon("📈", Brushes.Teal) },
            { "Okänd", new CategoryIcon("❓", Brushes.Gray) }
            /*
            { "Alla", "\uf0f6" },
            { "Food", "\uf2e7" },
            { "Transport", "\uf207" },
            { "Clothing", "\f553" },
            { "Taxes", "\\e56a" },
            { "House", "\uf015" },
            { "Hobbies", "\uE561" },
            { "Kids", "\f4d3" },
            { "TV", "\uE561" },
            { "SaaS", "\\e4e5" },
            { "Subscriptions", "\f415" },
            { "Pets", "\f6d3" },
            { "Entertainment", "\f008"},

            { "Salary", "\uf0d6" },
            { "Allowance", "\f81d" },
            { "ExtraIncome", "\f4c0" },
            { "Undefined", "\uf128" }
            */
        };


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string categoryName = value switch
            {
                string str => str,
                Category c => c.Name,
                _ => null
            };

            if (categoryName != null && _iconMap.TryGetValue(categoryName, out var icon))
            {
                if (targetType == typeof(string))
                    return icon.Emoji;
                if (targetType == typeof(Brush))
                    return icon.Color;

                return icon;
            }

            if (targetType == typeof(string)) return "❓";
            if (targetType == typeof(Brush)) return Brushes.Gray;

            return new CategoryIcon("❓", Brushes.Gray); // fallback icon
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}