using System;
using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel.Hot;
using Weibo.DataModel.Status;

namespace Weibo.Apis.V2
{
    public partial class Weibo
    {
        protected static async Task<RestResult<StatusWithUser[]>> GetStatusesAsync(string path)
        {
            return await HttpsGet<StatusWithUser[]>(path);

        }
        //permission denied
        public static async Task<RestResult<StatusWithUser[]>> 
            SearchTopicAsync(string q, long count, long page, string token)
        {
            var path = string.Format(@"search/topics.json?q={0}&count={1}&page={2}&access_token={3}", Uri.EscapeDataString(q), count, page, token);
            return await GetStatusAsync(path);
        }
        //no api
        public static async Task<RestResult<StatusWithUser[]>> 
            SearchStatusAsync(string q, long count, long page, string token, long consumerkey)
        {
            var path = string.Format("statuses/search.json?q={0}&page={1}&count={2}&source={3}", Uri.EscapeDataString(q), page, count, consumerkey);
            return await GetStatusAsync(path);
        }

        public static async Task<RestResult<StatusWithUser[]>> 
            RepostsDailyAsync(int count, string token)
        {
            var path = string.Format("statuses/hot/repost_daily.json?count={0}&access_token={1}", count, token);
            return await GetStatusesAsync(path);
        }

        public static async Task<RestResult<StatusWithUser[]>>
            RepostWeeklyAsync(int count, string token)
        {
            var path = string.Format("statuses/hot/repost_weekly.json?count={0}&access_token={1}", count, token);
            return await GetStatusesAsync(path);
        }

        public static async Task<RestResult<StatusWithUser[]>>
            CommentsDailyAsync(int count, string token)
        {
            var path = string.Format("statuses/hot/comments_daily.json?count={0}&access_token={1}"
                , count, token);
            return await GetStatusesAsync(path);
        }

        public static async Task<RestResult<StatusWithUser[]>>
            CommentsWeeklyAsync(int count, string token)
        {
            var path = string.Format("statuses/hot/comments_weekly.json?count={0}&access_token={1}"
                , count, token);
            return await GetStatusesAsync(path);
        }

        public static async Task<RestResult<StatusWithUser[]>> HotAsync(int type
            , int count
            , int ispic
            , int page, string token)
        {
            var path = string.Format("suggestions/statuses/hot.json?type={0}&count={1}&is_pic={2}&page={3}&access_token={4}"
                , type, count, ispic, page, token);
            var hses = await HttpsGet<HotStatuses>(path);
            var rtn = new RestResult<StatusWithUser[]>
            {
                StatusCode = hses.StatusCode,
                Error = hses.Error,
            };

            if (hses.Value == null || hses.Value.statuses == null 
                || hses.Value.statuses.Length == 0)
                return rtn;
            rtn.Value = new StatusWithUser[hses.Value.statuses.Length];
            var idx = 0;
            foreach (var hs in hses.Value.statuses)
            {
                hs.status.thumb_pic_width = hs.pwidth;
                hs.status.thumb_pic_height = hs.pheight;
                rtn.Value[idx++] = hs.status;
            }
            return rtn;
        }
    }
}
