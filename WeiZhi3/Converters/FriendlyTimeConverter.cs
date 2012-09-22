using System;
using System.Globalization;
using System.Windows.Data;

namespace WeiZhi3.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public sealed class FriendlyTimeConverter : IValueConverter
    {
        public static string PrintHour(DateTime tmt)
        {
            if (tmt.Hour < 6)
                return String.Format("凌晨{0}", tmt.Hour);
            if (tmt.Hour < 12)
                return String.Format("上午{0}", tmt.Hour);
            if (tmt.Hour == 12)
                return String.Format("下午{0}", tmt.Hour);
            if (tmt.Hour < 18)
                return String.Format("下午{0}", tmt.Hour - 12);
            return String.Format("晚{0}", tmt.Hour - 12);
        }
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tmt = (DateTime)value;
            if (tmt.Equals(DateTime.MinValue))
                return "已删除";

            var ts = DateTime.Now - tmt;
            var d = DateTime.Now.DayOfYear - tmt.DayOfYear;

            switch (d)
            {
            case 0:
                if (ts.Hours > 4)
                {
                    return PrintHour(tmt);
                }
                return ts.Hours > 0 ? String.Format("{0} 时", ts.Hours) : "<1 时";
            case 1:
                return "昨天";
            case 2:
                return "前天";
            default:
                return String.Format("{0}-{1}-{2}", tmt.Year - 2000, tmt.Month, tmt.Day);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}