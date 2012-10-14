using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using Weibo.Apis.SinaV2;

namespace Weibo.ViewModels
{
    public class HomeTimelineNavigationItem:NavigationItem
    {
        public HomeTimelineNavigationItem()
            : base("\uE10F")
        {
        }

        static long GetSinceId()
        {
            var mem = MemoryCache.Default;
            var sid = (long?) mem.Get("hometimeline_since_id");
            return sid.HasValue ? sid.Value : 0;
        }
        #region Overrides of NavigationItem

        public override async void OnTick(IWeiboAccessToken at)
        {
            if (_ticks++ % 17 != 0)
                return;
            var since_id = GetSinceId();
            if (since_id == 0)
                return;
            var resp = await WeiboClient.statuses_friends_timeline_ids_async(at.get(), since_id, 1, 100);
            if (resp.Failed() || resp.Value.statuses.Length == 0)
                return;
            
            //total_number = resp.Value.total_number;

            Debug.WriteLine("home-timeline total : {0},next:{1},prev:{2}", resp.Value.total_number, resp.Value.next_cursor, resp.Value.previous_cursor);
            var mid = resp.Value.statuses.Max();
            notifications = resp.Value.statuses.Length;
        }

        #endregion
    }
}