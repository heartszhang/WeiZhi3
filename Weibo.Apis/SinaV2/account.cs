using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel;

namespace Weibo.Apis.SinaV2
{
    public partial class WeiboClient
    {
        public static async Task<RestResult<RateLimitStatus>> account_rate_limit_status(string at)
        {
            var path = string.Format("account/rate_limit_status.json?access_token={0}", at);
            return await WeiboInternal.HttpsGet<RateLimitStatus>(WeiboSources.SinaV2(path));
        }
    }
}
