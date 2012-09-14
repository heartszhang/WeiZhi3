using System;
using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel.Status;
using Weibo.DataModel.UserModel;

namespace Weibo.Apis.V2
{
    public partial class Weibo
    {
        async static Task<RestResult<UserWithLatestStatus>> LoadUserProfileAsync(string path)
        {
            return await HttpsGet<UserWithLatestStatus>(path);
        }

        async public static Task<RestResult<UserWithLatestStatus>> ShowUserAync(long uid,string token)
        {
            var path = String.Format("users/show.json?uid={0}&access_token={1}", uid, token);
            return await LoadUserProfileAsync(path);
        }

        public static async Task<RestResult<StatusWithUser[]>>
            UserTimelineRefreshAsync(long userid, long sinceid, int count, int page, string token)
        {
            var path = String.Format("statuses/user_timeline.json?uid={0}&since_id={1}&count={2}&page={3}&access_token={4}"
                , userid, sinceid, count, page, token);
            return await GetStatusAsync(path);
        }

        public static async Task<RestResult<StatusWithUser[]>>
            UserTimelineNextPageAsync(long userid, long maxid, int page, int count, string token)
        {
            var path = String.Format("statuses/user_timeline.json?uid={0}&max_id={1}&count={2}&page={3}&access_token={4}", userid, maxid, count, page, token);
            return await GetStatusAsync(path);
        }

        public static async Task<RestResult<StatusWithUser[]>>
            UserTimelineRefreshAsync(string screenname, long sinceid, int count, int page, string token)
        {
            var path = String.Format("statuses/user_timeline.json?uid={0}&since_id={1}&count={2}&page={3}&access_token={4}"
                , screenname, sinceid, count, page, token);
            return await GetStatusAsync(path);
        }

        public static async Task<RestResult<StatusWithUser[]>>
            UserTimelineNextPageAsync(string screenname, long maxid, int page, int count, string token)
        {
            var path = String.Format("statuses/user_timeline.json?uid={0}&max_id={1}&count={2}&page={3}&access_token={4}", screenname, maxid, count, page, token);
            return await GetStatusAsync(path);
        }
    }
}