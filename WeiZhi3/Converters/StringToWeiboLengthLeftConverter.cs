using System;
using System.Globalization;
using System.Windows.Data;

namespace WeiZhi3.Converters
{
    [ValueConversion(typeof(string), typeof(int))]
    public class StringToWeiboLengthLeftConverter : IValueConverter
    {
        // Methods
        public object Convert(object value, Type target_type, object parameter, CultureInfo culture)
        {
            var v = (string)value;
            var l = string.IsNullOrEmpty(v) ? 0 : v.Length;
            return 140 - l;
        }

        public object ConvertBack(object value, Type target_type, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}