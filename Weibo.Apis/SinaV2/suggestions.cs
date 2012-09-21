using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel;

namespace Weibo.Apis.SinaV2
{
    public partial class WeiboClient
    {
        public static async Task<RestResult<Status[]>> suggestions_statuses_hot_async(int type
            , int count
            , int ispic
            , int page, string token)
        {
            var path = string.Format("suggestions/statuses/hot.json?type={0}&count={1}&is_pic={2}&page={3}&access_token={4}"
                , type, count, ispic, page, token);
            var hses = await WeiboInternal.HttpsGet<HotStatuses>(path);
            var rtn = new RestResult<Status[]>
            {
                StatusCode = hses.StatusCode,
                Reason = hses.Reason,
            };

            if (hses.Value == null || hses.Value.statuses == null
                || hses.Value.statuses.Length == 0)
                return rtn;
            rtn.Value = new Status[hses.Value.statuses.Length];
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
