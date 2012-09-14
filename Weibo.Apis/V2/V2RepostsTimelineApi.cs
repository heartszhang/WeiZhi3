using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel.Status;

namespace Weibo.Apis.V2
{
    public partial class Weibo
    {
        static async Task<RestResult<StatusWithUser[]>> LoadRepostStatusAsync(string path)
        {
            var r = await HttpsGet<Reposts>(path);
            var rtn = new RestResult<StatusWithUser[]>
            {
                StatusCode = r.StatusCode,Error = r.Error,
                Value = r.Value == null ? null : r.Value.reposts,
            };
            return rtn;
        }

        public static async Task<RestResult<StatusWithUser[]>> 
            RepostTimelineRefreshAsync(long statusid, long sinceid, int page, int count, string token)
        {
            var path = string.Format("statuses/repost_timeline.json?id={0}&since_id={1}&count={2}&page={3}&access_token={4}", statusid, sinceid, count, page, token);

            return await LoadRepostStatusAsync(path);
        }
        public static async Task<RestResult<StatusWithUser[]>> 
            RepostTimelineNextPageAsync(long statusid, long maxid, int page, int count, string token)
        {
            var path = string.Format("statuses/repost_timeline.json?id={0}&max_id={1}&count={2}&page={3}&access_token={4}", statusid, maxid, count, page, token);

            return await LoadRepostStatusAsync(path);
        }
    }
}