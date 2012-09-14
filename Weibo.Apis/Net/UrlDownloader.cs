using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using WeiZhi.Base;

namespace Weibo.Apis.Net
{
    public static class UrlDownloader
    {
        private static readonly Random seed = new Random();

        async internal static Task<string> DownloadUrlAsync(string url, string type, string accept, string uid)
        {
            if (File.Exists(uid))
                return uid;
            var fp = uid + '.' + seed.Next();
            using(var wc = new WebClient())
            {
                try
                {
                    if (!string.IsNullOrEmpty(accept))
                        wc.Headers[HttpRequestHeader.Accept] = accept;
                    await wc.DownloadFileTaskAsync(url, fp);

                    var ct = wc.ResponseHeaders[HttpResponseHeader.ContentType];
                    var encod = wc.ResponseHeaders[HttpResponseHeader.ContentLength];
                    if (!string.IsNullOrEmpty(type) && !ct.Contains(type))
                    {
                        File.Delete(fp);
                        return string.Empty;
                    }
                    if (!File.Exists(uid))
                        try { File.Copy(fp, uid, false); }
                        catch (IOException) { }
                    File.Delete(fp);
                    return uid;
                }
                catch (UriFormatException e)
                {
                    Debug.WriteLine(e.Message, url);
                }
                catch (WebException e)
                {
                    Debug.WriteLine(e.Message, url);
                }
            }
            return string.Empty;
        }
        public static async Task<string> DownloadThumbnailAsync(string url)
        {
            return await DownloadPicAsync(url, "thumbnails");
        }

        public static async Task<string> DownloadAvatarAsync(string url)
        {
            return await DownloadPicAsync(url, "avatars");
        }

        public static async Task<string> DownloadMiddleAsync(string url)
        {
            return await DownloadPicAsync(url, "middles");
        }

        async static Task<string> DownloadPicAsync(string url, string catalog)
        {
            var uid = WeiZhiDirectories.CacheFilePath(catalog, url, ".jpg");
            return await DownloadUrlAsync(url, "image", null, uid);
        }

        public async static Task<string> DownloadHtmlAsync(string url)
        {
            var path = WeiZhiDirectories.HtmlPath();

            var uid = path + WeiZhiDirectories.hash_file_name(url) + ".html";
            return await DownloadUrlAsync(url, "text", "text/html", uid);
        }

        public static string DownloadHtmlSync(string url)
        {
            var rtn = DownloadHtmlAsync(url);

            var r = rtn.Wait(TimeSpan.FromSeconds(20));
            return r ? rtn.Result : string.Empty;
        }
    }
}