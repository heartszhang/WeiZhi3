using System.Globalization;
using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel;

namespace Weibo.Apis.SinaV2
{
    public partial class WeiboClient
    {
        public static async Task<RestResult<User>>
            friendships_destroy_async(long uid, string token)
        {
            return await
                    WeiboInternal.HttpsPost<User>("friendships/destroy.json",
                        new System.Collections.Specialized.NameValueCollection
                        {
                            {"access_token",token},
                            {"uid",uid.ToString(CultureInfo.InvariantCulture)},
                        });

        }
        public static async Task<RestResult<User>>
            friendships_create_async(long uid, string token)
        {
            return await
                    WeiboInternal.HttpsPost<User>("friendships/create.json",
                        new System.Collections.Specialized.NameValueCollection
                        {
                            {"access_token",token},
                            {"uid",uid.ToString(CultureInfo.InvariantCulture)},
                        });

        }

        public static async Task<RestResult<Users>>
            friendships_followers_async(long userid,
            int count, int cursor, string token)
        {
            var path = string.Format("friendships/followers.json?count={0}&access_token={1}&uid={2}&cursor={3}",
                                     count,
                                     token,
                                     userid, cursor);
            return await WeiboInternal.HttpsGet<Users>(path);
        }
        public static async Task<RestResult<Users>>
            friendships_followers_active_async(long userid,
            int count, string token)
        {
            var path = string.Format("friendships/followers/active.json?count={0}&access_token={1}&uid={2}",
                                     count,
                                     token,
                                     userid);
            return await WeiboInternal.HttpsGet<Users>(path);
        }
        public static async Task<RestResult<Users>>
            friendships_friends_chain_followers_async(long userid,
            int count, int page, string token)
        {
            var path = string.Format("friendships/friends_chain/followers.json?count={0}&access_token={1}&uid={2}&page={3}",
                                     count,
                                     token,
                                     userid, page);
            return await WeiboInternal.HttpsGet<Users>(path);

        }
        public static async Task<RestResult<Users>>
            friendships_friends_async(long userid, int count, int cursor, string token)
        {
            var path = string.Format("friendships/friends.json?count={0}&cursor={1}&uid={2}&access_token={3}&trim_status=0"
                , count, cursor, userid, token);
            return await WeiboInternal.HttpsGet<Users>(path);
        }

        public static async Task<RestResult<Users>>
            friendships_friends_in_common_async(long userid, int count, int page, string token)
        {
            var path = string.Format("friendships/friends/in_common.json?count={0}&page={1}&uid={2}&access_token={3}"
                , count, page, userid, token);
            return await WeiboInternal.HttpsGet<Users>(path);
        }

        public static async Task<RestResult<Users>>
            friendships_friends_bilateral_async(long userid, int count, int page, string token)
        {
            var path = string.Format("friendships/friends/bilateral.json?count={0}&page={1}&uid={2}&access_token={3}"
                , count, page, userid, token);
            return await WeiboInternal.HttpsGet<Users>(path);
        }

        public static async Task<RestResult<UserIds>> friendships_followers_ids_async(long userid,
            int count, string token)
        {
            var path = string.Format("friendships/followers/ids.json?uid={0}&count={1}&access_token={2}",
                                     userid,
                                     count,
                                     token);

            return await WeiboInternal.HttpsGet<UserIds>(path);
        }
    }
}
