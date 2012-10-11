using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel;

namespace Weibo.Apis.SinaV2
{
    public enum hot_status_type
    {
        reserved = 0,
        musement = 1,
        funny = 2,
        beauty = 3,
        video = 4,
        star = 5,
        naive = 6,
        fashion = 7,
        mobile = 8,
        food = 9,
        music = 10,
        count = 10,
    }
    public partial class WeiboClient
    {
        public static async Task<RestResult<Status[]>> suggestions_statuses_hot_async(string token
            , int count
            , bool ispic
            , int page, hot_status_type type)
        {
            var path = string.Format("suggestions/statuses/hot.json?type={0}&count={1}&is_pic={2}&page={3}&access_token={4}"
                , (int)type, count, ispic ? 1 : 0, page, token);
            var hses = await WeiboInternal.HttpsGet<HotStatuses>(WeiboSources.SinaV2(path));
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
