using System;
using System.Windows;
using System.Windows.Data;

namespace WeiZhi3.Converters
{
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class StringToVisibiltyConverter : IValueConverter
    {
        // Methods
        public object Convert(object value, Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            var val = (string)value;
            return string.IsNullOrEmpty(val) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}