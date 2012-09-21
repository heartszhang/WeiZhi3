using System;
using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel;

namespace Weibo.Apis.SinaV2
{
    public partial class WeiboClient
    {
        async public static Task<RestResult<User>> users_show_async(long uid, string token)
        {
            var path = String.Format("users/show.json?uid={0}&access_token={1}", uid, token);
            return await WeiboInternal.HttpsGet<User>(WeiboSources.SinaV2(path));
        }
    }
}
