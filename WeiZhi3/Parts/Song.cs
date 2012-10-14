using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Weibo.Apis.SinaV2;
using Weibo.DataModel;

namespace WeiZhi3.Parts
{
    internal class Song : DependencyObject
    {
        public string Author { get; set; }
        public string Album { get; set; }
        public string Title { get; set; }
        public string UrlShort { get; set; }
        public string Pic { get; set; }

        internal Song(UrlInfo ui)
        {
            Debug.Assert(ui != null && ui.annotations != null && ui.annotations.Length > 0);
            Author = ui.annotations[0].author;
            Album = ui.annotations[0].album;
            Pic = ui.annotations[0].pic;
            UrlShort = ui.url_short;
            Title = ui.topic();
        }
        private string ShortPath()
        {
            return UrlShort.Remove(0, Properties.Resources.ShortUrlPrefix.Length);
        }

        internal async Task<string> FetchMp3()
        {
            var rlt = await WeiboClient.widget_html5_show(ShortPath(), 0);
            return rlt.Failed() ? null : rlt.Value.mp4();
        }
    }
}