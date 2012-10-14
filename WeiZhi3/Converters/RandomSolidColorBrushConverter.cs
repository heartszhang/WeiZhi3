using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WeiZhi3.Converters
{
    public class RandomSolidColorBrushConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush(App.RandomColor());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}