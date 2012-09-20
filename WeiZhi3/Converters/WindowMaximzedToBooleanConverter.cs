using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WeiZhi3.Converters
{
    internal class WindowMaximzedStateToBooleanConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (WindowState)value;
            return s == WindowState.Maximized;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var maximized = (bool)value;
            return maximized ? WindowState.Maximized : WindowState.Normal;
        }

        #endregion
    }

    [ValueConversion(typeof(string), typeof(Visibility))]
    public class ImageUrlConverter : IValueConverter
    {
        // Methods
        public object Convert(object value, Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            var url = (string) value;
            return string.IsNullOrEmpty(url) ? null : new Uri(url);
        }

        public object ConvertBack(object value, Type target_type, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
