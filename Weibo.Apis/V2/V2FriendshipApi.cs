using System.Globalization;
using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel.UserModel;

namespace Weibo.Apis.V2
{
    public partial class Weibo
    {
        public static async Task<RestResult<UserWithLatestStatus>> 
            FriendshipsDestroyAsync(long uid,string token)
        {            
            return    await
                    HttpsPost<UserWithLatestStatus>("friendships/destroy.json",
                        new System.Collections.Specialized.NameValueCollection
                        {
                            {"access_token",token},
                            {"uid",uid.ToString(CultureInfo.InvariantCulture)},
                        });

        }
        public static async Task<RestResult<UserWithLatestStatus>>
            FriendshipsCreateAsync(long uid, string token)
        {
            return await
                    HttpsPost<UserWithLatestStatus>("friendships/create.json",
                        new System.Collections.Specialized.NameValueCollection
                        {
                            {"access_token",token},
                            {"uid",uid.ToString(CultureInfo.InvariantCulture)},
                        });

        }

        public static async Task<RestResult<Users>>
            FriendshipsFollowersAsync(long userid,
            int count, int cursor, string token)
        {
            var path = string.Format("friendships/followers.json?count={0}&access_token={1}&uid={2}&cursor={3}",
                                     count,
                                     token,
                                     userid, cursor);
            return await HttpsGet<Users>(path);
        }
        public static async Task<RestResult<Users>>
            FriendshipsFollowersActiveAsync(long userid,
            int count, string token)
        {
            var path = string.Format("friendships/followers/active.json?count={0}&access_token={1}&uid={2}",
                                     count,
                                     token,
                                     userid);
            return await HttpsGet<Users>(path);
        }
        public static async Task<RestResult<Users>>
            FriendshipsFriendsChainFollowersAsync(long userid,
            int count, int page, string token)
        {
            var path = string.Format("friendships/friends_chain/followers.json?count={0}&access_token={1}&uid={2}&page={3}",
                                     count,
                                     token,
                                     userid,page);
            return await HttpsGet<Users>(path);

        }
        public static async Task<RestResult<Users>>
            FriendshipsFriendsAsync(long userid, int count, int cursor, string token)
        {
            var path = string.Format("friendships/friends.json?count={0}&cursor={1}&uid={2}&access_token={3}&trim_status=0"
                ,count, cursor, userid, token);
            return await HttpsGet<Users>(path);
        }

        public static async Task<RestResult<Users>>
            FriendshipsFriendsInCommonAsync(long userid, int count, int page, string token)
        {
            var path = string.Format("friendships/friends/in_common.json?count={0}&page={1}&uid={2}&access_token={3}"
                , count, page, userid, token);
            return await HttpsGet<Users>(path);
        }

        public static async Task<RestResult<Users>>
            FriendshipsFriendsBilateralAsync(long userid, int count, int page, string token)
        {
            var path = string.Format("friendships/friends/bilateral.json?count={0}&page={1}&uid={2}&access_token={3}"
                , count, page, userid, token);
            return await HttpsGet<Users>(path);
        }

        public static async Task<RestResult<UserIds>> FriendshipsFollowersIdsRefreshAsync(long userid,
            int count,string token)
        {
            var path = string.Format("friendships/followers/ids.json?uid={0}&count={1}&access_token={2}",
                                     userid,
                                     count,
                                     token);

            return await HttpsGet<UserIds>(path);
        }
    }
}