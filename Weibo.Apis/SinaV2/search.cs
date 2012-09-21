using System;
using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel;

namespace Weibo.Apis.SinaV2
{
    public partial class WeiboClient
    {
        //permission denied
        public static async Task<RestResult<Status[]>>
            search_topics_async(string q, long count, long page, string token)
        {
            var path = string.Format(@"search/topics.json?q={0}&count={1}&page={2}&access_token={3}", Uri.EscapeDataString(q), count, page, token);
            return await WeiboInternal.HttpsGet<Status[]>(path);
        }


    }
}
