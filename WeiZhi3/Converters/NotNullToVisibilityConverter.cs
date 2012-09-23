using System;
using System.Windows;
using System.Windows.Data;

namespace WeiZhi3.Converters
{
    public class NotNullToVisibilityConverter : IValueConverter
    {
        // Methods
        public object Convert(object value, Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class NullToVisibilityConverter : IValueConverter
    {
        // Methods
        public object Convert(object value, Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}