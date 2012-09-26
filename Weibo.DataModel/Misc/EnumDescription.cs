using System;
using System.ComponentModel;
using System.Reflection;

namespace Weibo.DataModel.Misc
{
    public class EnumDescription
    {
        public static string Get(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            if (fi == null)
                return value.ToString();
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }        
    }
}