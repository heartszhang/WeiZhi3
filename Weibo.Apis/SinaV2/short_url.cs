using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel;

namespace Weibo.Apis.SinaV2
{
    public partial class WeiboClient
    {
        public static async Task<RestResult<UrlInfos>> short_url_info(IEnumerable<string> shorturls, long consumerkey)
        {
            var urls = shorturls.Aggregate(string.Empty
                , (current, shorturl) =>
                    current + string.Format("&url_short={0}", Uri.EscapeDataString(shorturl)));
            if(consumerkey == 0)
                consumerkey = 603152360;//微游戏:603152360
            var path = string.Format("short_url/info.json?source={0}{1}", consumerkey, urls);

            return await WeiboInternal.HttpsGet<UrlInfos>(WeiboSources.SinaV2(path));
        }
        public static async Task<UrlShorts> short_url_expand(IEnumerable<string> shorturls, long consumerkey)
        {
            var urls = shorturls.Aggregate(string.Empty
                , (current, shorturl) =>
                    current + string.Format("&url_short={0}", Uri.EscapeDataString(shorturl)));
            var path = string.Format("short_url/expand.json?source={0}{1}", consumerkey, urls);
            return (await WeiboInternal.HttpsGet<UrlShorts>(WeiboSources.SinaV2(path))).Value;
        }
        public static async Task<string> widget_show(string shorturl, long consumerkey)
        {
            if (consumerkey == 0)
                consumerkey = 603152360;//微游戏:603152360
            var path = string.Format("widget/show.jsonp?short_url={0}&source={1}"
                , Uri.EscapeDataString(shorturl), consumerkey);
            var wsh = await WeiboInternal.HttpsGet<WidgetShow>(WeiboSources.SinaV1(path));
            return wsh.Value != null ? wsh.Value.result : null;
        }

        //for music address
        //http://api.weibo.com/widget/show.jsonp?short_url=zOxJ9Y3&template_name=html5&source=3818214747
        //&template_name=embed, 使用html5的返回格式，可以使用media-element播放
        public static async Task<string> widget_html5_show(string shorturl, long consumerkey)
        {
            if (consumerkey == 0)
                consumerkey = 603152360;//微游戏:603152360
            var path = string.Format("widget/show.jsonp?short_url={0}&template_name=html5&source={1}"
                , Uri.EscapeDataString(shorturl), consumerkey);
            var wsh = await WeiboInternal.HttpsGet<WidgetShow>(WeiboSources.SinaV1(path));
            return wsh.Value != null ? wsh.Value.result : null;
        }


    }
}
