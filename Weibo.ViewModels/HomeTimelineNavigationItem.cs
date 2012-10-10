using System.Diagnostics;
using System.Linq;
using Weibo.Apis.SinaV2;

namespace Weibo.ViewModels
{
    public class HomeTimelineNavigationItem:NavigationItem
    {
        public HomeTimelineNavigationItem()
            : base("\uE10F")
        {
        }

        #region Overrides of NavigationItem

        public override async void OnTick(IWeiboAccessToken at)
        {
            if (_ticks++ % 17 != 0)
                return;
            var resp = await WeiboClient.statuses_friends_timeline_ids_async(at.get(), _since_id, 1, 100);
            if (resp.Failed())
                return;

            //total_number = resp.Value.total_number;

            Debug.WriteLine("home-timeline total : {0},next:{1},prev:{2}", resp.Value.total_number, resp.Value.next_cursor, resp.Value.previous_cursor);
            var mid = resp.Value.statuses.Max();
            if (_since_id < mid)
                _since_id = mid;
            total_number = resp.Value.statuses.Length;
        }

        #endregion
    }
}