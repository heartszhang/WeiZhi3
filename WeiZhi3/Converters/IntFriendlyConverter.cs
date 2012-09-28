using System;
using System.Globalization;
using System.Windows.Data;

namespace WeiZhi3.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class IntFriendlyConverter : IValueConverter
    {
        // Methods
        public object Convert(object value, Type target_type, object parameter, CultureInfo culture)
        {
            var v = (int)value;
            return v > 99999 ? string.Format("{0}Íò+", v / 10000) : v.ToString(CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type target_type, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}