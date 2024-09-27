using System.Globalization;
using System.Windows.Data;

namespace CHAT_APP_CLIENT.Extensions
{
    public class CustomMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // Handle null values safely
            if (values == null || values.Length == 0)
                return string.Empty;

            return string.Join(" ", values.Where(v => v != null));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new object[targetTypes.Length];

            return value.ToString()?.Split(' ') ?? new object[targetTypes.Length];
        }
    }
}