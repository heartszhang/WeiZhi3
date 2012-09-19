using System;

namespace Weibo.ViewModels.DataModels
{
    public static class DateTimeHelper
    {
        public static long UnixTimestamp(this DateTime t)
        {
            return (long)(t - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}