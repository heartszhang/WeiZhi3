using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel;

namespace Weibo.Apis.SinaV2
{
    public partial class WeiboClient
    {
        public static async Task<RestResult<RemindUnreadCount>>
            remind_unread_count(long uid, long consumerkey, string token)
        {
            var path = string.Format("remind/unread_count.json?uid={0}&source={1}&access_token={2}",
                                     uid,
                                     consumerkey,
                                     token);
            return await WeiboInternal.HttpsGet<RemindUnreadCount>(path);
        }
    }
}
