using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel.Status;

namespace Weibo.Apis.V2
{
    public partial class Weibo
    {
        public static async Task<RestResult<StatusWithUser[]>> FriendsTimelineRefreshAync(long sinceid,int page, int count,string token)
        {
            var path = string.Format("statuses/friends_timeline.json?count={0}&since_id={1}&access_token={2}&page={3}"
                , count, sinceid, token,page);
            return await GetStatusAsync(path);
        }

        public static async Task<RestResult<StatusWithUser[]>> FriendsTimelineNextPageAsync(int page, long maxid, int count, string token)
        {            
            return await GetStatusAsync(string.Format("statuses/friends_timeline.json?count={0}&page={1}&feature={2}&max_id={3}&access_token={4}", count, page, 0, maxid, token));
        }

        public static async Task<RestResult<StatusWithUser[]>> MentionsMeRefreshAsync(long sinceid,
            int count,
            int page,string token)
        {
            var path = string.Format("statuses/mentions.json?page={0}&count={1}&since_id={2}&access_token={3}",
                                     page,
                                     count,
                                     sinceid,
                                     token);
            return await GetStatusAsync(path);
        }
        public static async Task<RestResult<StatusIds>> MentionsCountIdsAsync(long sinceid, int count, string token)
        {
            var path = string.Format("statuses/mentions/ids.json?since_id={0}&access_token={1}&count={2}", sinceid, token, count);
            return await HttpsGet<StatusIds>(path);
        }
        public static async Task<RestResult<StatusIds>> FriendsTimelineIdsAsync(long sinceid, int page,int count, string token)
        {
            var path = string.Format("statuses/friends_timeline/ids.json?since_id={0}&access_token={1}&count={2}&page={3}", sinceid, token, count,page);
            return await HttpsGet<StatusIds>(path);
        }        
    }
}