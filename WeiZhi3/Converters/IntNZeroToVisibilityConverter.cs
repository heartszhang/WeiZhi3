using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WeiZhi3.Converters
{
    public class IntNZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var i = (int)value;
            return i != 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}