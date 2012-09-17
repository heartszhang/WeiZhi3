using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Weibo.Api2
{
    internal struct WeiboRequestHandler
    {
        internal static readonly string[] BaseUrl = { "https://api.weibo.com/2/", "http://open.t.qq.com/api/" };
        internal int RetryTimes ;
        internal int Timeout;
        internal string UrlPath;
        internal ExpandoObject Parameters;
        internal WeiboRequestHandler(string path , ExpandoObject paras = null, int retry = 2, int timeo = 30000):this()
        {
            RetryTimes = retry;
            Timeout = timeo;
            UrlPath = path;
            Parameters = paras;
        }
        internal string CreateUrl(WeiboSources src)
        {
            if(Parameters == null || (Parameters as IDictionary<string,object>).Count == 0)
                return BaseUrl[(int) src] + UrlPath;
            var query = Parameters.Aggregate(string.Empty, 
                                             (current, prop) => current + ("&" + prop.Key + "=" + Uri.EscapeDataString(prop.Value.ToString())));

            if (!string.IsNullOrEmpty(query))
            {
                query = query.Remove(0, 1);
            }

            return BaseUrl[(int)src] + UrlPath + "?" + query;
        }
    }
}