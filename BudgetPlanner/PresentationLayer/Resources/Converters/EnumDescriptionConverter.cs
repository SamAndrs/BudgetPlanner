using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace BudgetPlanner.PresentationLayer.Resources.Converters
{
    class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           if(value == null) return string.Empty;

           Type type = value.GetType();

            if (!type.IsEnum)
                return value.ToString()!; // fallback

            string name = value.ToString();
            FieldInfo field = type.GetField(name);

            if(field == null)
                return name;// fallback

            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute != null ? attribute.Description : name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;

            foreach (var field in targetType.GetFields())
            {
                DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();

                if ((attribute?.Description ?? field.Name) == value.ToString())
                    return Enum.Parse(targetType, field.Name);
            }

            return Binding.DoNothing;
        }
    }
}
