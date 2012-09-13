using System;

namespace WeiZhi3.DataModel
{
    internal static class DateTimeHelper
    {
        internal static long UnixTimestamp(this DateTime t)
        {
            return (long)(t - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}