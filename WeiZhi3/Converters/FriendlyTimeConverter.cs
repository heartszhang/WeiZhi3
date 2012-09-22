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
                return String.Format("�賿{0}", tmt.Hour);
            if (tmt.Hour < 12)
                return String.Format("����{0}", tmt.Hour);
            if (tmt.Hour == 12)
                return String.Format("����{0}", tmt.Hour);
            if (tmt.Hour < 18)
                return String.Format("����{0}", tmt.Hour - 12);
            return String.Format("��{0}", tmt.Hour - 12);
        }
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tmt = (DateTime)value;
            if (tmt.Equals(DateTime.MinValue))
                return "��ɾ��";

            var ts = DateTime.Now - tmt;
            var d = DateTime.Now.DayOfYear - tmt.DayOfYear;

            switch (d)
            {
            case 0:
                if (ts.Hours > 4)
                {
                    return PrintHour(tmt);
                }
                return ts.Hours > 0 ? String.Format("{0} ʱ", ts.Hours) : "<1 ʱ";
            case 1:
                return "����";
            case 2:
                return "ǰ��";
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