using System;
using System.Globalization;
using System.Windows.Data;
using Weibo.ViewModels;

namespace WeiZhi3.Converters
{
    [ValueConversion(typeof(bool),typeof(string))]
    public sealed class FollowCommandConverter : IValueConverter
    {
        private readonly string _screen_name;
        public FollowCommandConverter(string user)
        {
            _screen_name = user;
        }

        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var f = (bool) value;
            return string.Format(f ? "取消关注 @{0}" : "关注 @{0}", _screen_name);
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}