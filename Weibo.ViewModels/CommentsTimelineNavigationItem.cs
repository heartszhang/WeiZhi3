using System.Diagnostics;
using Weibo.Apis.Net;
using Weibo.Apis.SinaV2;

namespace Weibo.ViewModels
{
    public class CommentsTimelineNavigationItem : NavigationItem
    {
        public CommentsTimelineNavigationItem()
            : base("\uE1A6")
        {
        }

        #region Overrides of NavigationItem

        public override async void OnTick(IWeiboAccessToken at)
        {
            if (_ticks++ % 23 != 0)
                return;
            var resp = await WeiboClient.comments_to_me_refresh_async(at.get(), _since_id, 5, 1);
            if (resp.Failed())
                return;
            var popimgref = 0;
            var popular_image = string.Empty;
            var recent_text = string.Empty;

            total_number = resp.Value.total_number;

            Debug.WriteLine("comments-timeline total : {0},next:{1},prev:{2}", resp.Value.total_number,resp.Value.next_cursor,resp.Value.previous_cursor);
            foreach (var c in resp.Value.comments)
            {
                if (_since_id < c.id)
                    _since_id = c.id;
                if (recent_text.Length < c.text.Length)
                    recent_text = c.text;
                if (c.status == null)
                    continue;
                if (!string.IsNullOrEmpty(c.status.thumbnail_pic) && c.status.comments_count > popimgref)
                {
                    popular_image = c.status.thumbnail_pic;
                    popimgref = c.status.comments_count;
                }
                if (c.status.retweeted_status == null)
                    continue;
                if ( c.status.retweeted_status.comments_count > popimgref
                    && !string.IsNullOrEmpty(c.status.retweeted_status.thumbnail_pic))
                {
                    popular_image = c.status.retweeted_status.thumbnail_pic;
                    popimgref = c.status.retweeted_status.comments_count;
                }
            }
            if (!string.IsNullOrEmpty(popular_image))
                image = popular_image;
            if (!string.IsNullOrEmpty(recent_text))
                text = recent_text;

        }

        #endregion
    }
}