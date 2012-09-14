using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel.Misc;
using Weibo.DataModel.Remind;
using Weibo.DataModel.Url;

namespace Weibo.Apis.V2
{
    public partial class Weibo 
    {
        public static async Task<UrlShorts> ShortUrlExpandAsync(IEnumerable<string> shorturls, long consumerkey)
        {
            var urls = shorturls.Aggregate(string.Empty
                , (current, shorturl) =>
                    current + string.Format("&url_short={0}", Uri.EscapeDataString(shorturl)));
            var path = string.Format("short_url/expand.json?source={0}{1}", consumerkey, urls);
            return (await HttpsGet<UrlShorts>(path)).Value;
        }
        public static async Task<string> WidgetShowAsync(string shorturl, long consumerkey)
        {
            var path = string.Format("widget/show.jsonp?short_url={0}&source={1}"
                , Uri.EscapeDataString(shorturl), consumerkey);
            var wsh = await HttpsGet<WidgetShow>(path, "http://api.weibo.com");
            return wsh.Value != null ? wsh.Value.result : null;
        }

        //for music address
        //http://api.weibo.com/widget/show.jsonp?short_url=zOxJ9Y3&template_name=html5&source=3818214747
        //&template_name=embed, 使用html5的返回格式，可以使用media-element播放
        public static async Task<string> WidgetHtml5ShowAsync(string shorturl, long consumerkey)
        {
            var path = string.Format("widget/show.jsonp?short_url={0}&template_name=html5&source={1}"
                , Uri.EscapeDataString(shorturl), consumerkey);
            var wsh = await HttpsGet<WidgetShow>(path, "http://api.weibo.com");
            return wsh.Value != null ? wsh.Value.result : null;
        }

        public static async Task<RestResult<RemindUnreadCount>> 
            RemindUnreadCountAsync(long uid, long consumerkey, string token)
        {
            var path = string.Format("remind/unread_count.json?uid={0}&source={1}&access_token={2}",
                                     uid,
                                     consumerkey,
                                     token);
            return await HttpsGet<RemindUnreadCount>(path);
        }

    }
}