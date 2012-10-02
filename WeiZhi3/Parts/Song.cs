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
            return UrlShort.Remove(0, "http://t.cn/".Length);
        }

        internal async Task<string> FetchMp3()
        {
            var rlt = await WeiboClient.widget_html5_show(ShortPath(), 0);
            /*            using (var client = new HttpClient())
                        {
                            var url =
                                string.Format(
                                    "http://api.weibo.com/widget/show.jsonp?ver=3&template_name=html5&source=3818214747&short_url={0}",
                                    ShortPath());
                            var resp = await client.GetStringAsync(url);
                            dynamic data = JsonValue.Parse(resp);
                            string result = data.result;
                            var doc = new XmlDocument { XmlResolver = null };
                            doc.LoadXml(result.Replace(@"&", @"&amp;"));//不知道为什么需要转义这里的&,而浏览器不需要转义
                            var param = doc.SelectSingleNode(@"//video/@src") as XmlAttribute;

                            return param != null ? param.Value : null;
                        }
                    
             * */
            return rlt.Failed() ? null : rlt.Value.mp4();
        }
    }
}