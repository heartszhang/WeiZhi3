using System;
using System.Windows;
using System.Windows.Data;
using Weibo.DataModel;
using Weibo.DataModel.Misc;

namespace WeiZhi3.Converters
{
    //[ValueConversion(typeof(UrlInfo), typeof(Visibility))]
    //public class UriInfoToHasDocumentConverter : IValueConverter
    //{
    //    // Methods
    //    public object Convert(object value, Type target_type, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        var url = (UrlInfo)value;
    //        return url != null && (url.type == UrlShortType.Normal || url.type == UrlShortType.Blog 
    //            || url.type == UrlShortType.News) ? Visibility.Visible : Visibility.Collapsed;
    //    }

    //    public object ConvertBack(object value, Type target_type, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}