using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weibo.Api2.Sina
{
    public partial class SinaClient : WeiboClient
    {
        /* short_url/info
      "result": true,
      "last_modified": 1347453752,
      "title": "",
      "description": "",
      "url_short": "http://t.cn/zlvquBd",
      "annotations": [],
      "url_long": "http://product.m18.com/p-R124395.htm",
      "type": 0
         * */
        public static async Task<WeiboResponse> short_url_info(IEnumerable<object> urls)
        {
            dynamic paras = new ExpandoObject();
            paras.source = "603152360";//微游戏:603152360

            return  await WeiboClientInternal.WeiboGet(new WeiboRequestHandler("short_url/info.json")
            {
                Parameters = paras,
                ListName = "url_short",
                ValueArray = urls.ToList(),
            }, WeiboSources.Sina);            
        }
    }
}
