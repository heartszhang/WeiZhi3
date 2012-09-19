using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Weibo.Api2
{
    internal struct WeiboRequestHandler
    {
        internal static readonly string[] BaseUrl = { "https://api.weibo.com/2/", "http://open.t.qq.com/api/", "https://api.weibo.com/" };
        internal int RetryTimes ;
        internal int Timeout;
        internal string UrlPath;
        internal ExpandoObject Parameters;
        internal string ListName;
        internal List<object> ValueArray;

        internal WeiboRequestHandler(string path, ExpandoObject paras = null, int retry = 2, int timeo = 30000)
            : this()
        {
            RetryTimes = retry;
            Timeout = timeo;
            UrlPath = path;
            Parameters = paras;
         //   ListName = null;
         //   ValueArray = null;
        }
        internal static WeiboRequestHandler Create(string path, ExpandoObject paras = null, string listname = null, 
            IEnumerable<object> values = null,int retry = 2, int timeo = 30000)
        {
            var rtn= new WeiboRequestHandler()
                {
                    RetryTimes =  retry,
                    Timeout = timeo,
                    UrlPath = path,
                    Parameters = paras,
                    ListName = listname,
                    ValueArray = values == null ? null : values.ToList(),
                };
            return rtn;
        }
        internal string CreateUrl(WeiboSources src)
        {
            if(Parameters == null || (Parameters as IDictionary<string,object>).Count == 0)
                return BaseUrl[(int) src] + UrlPath;
            var query = Parameters.Aggregate(string.Empty, 
                                             (current, prop) => current + ("&" + prop.Key + "=" + Uri.EscapeDataString(prop.Value.ToString())));

            if (!string.IsNullOrEmpty(ListName) && ValueArray != null && ValueArray.Count > 0)
            {
                var tmp_this = this;
                query = tmp_this.ValueArray.Aggregate(query, (current, v) => current + ("&" + tmp_this.ListName + "=" + Uri.EscapeDataString(v.ToString())));
            }
            if (!string.IsNullOrEmpty(query))
            {
                query = query.Remove(0, 1);
            }

            return BaseUrl[(int)src] + UrlPath + "?" + query;
        }
    }
}