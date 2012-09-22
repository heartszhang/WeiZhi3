using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Weibo.Apis.Net
{
    public static class HttpDownloadToLocalFile
    {
        private static readonly Random Seed = new Random();

        internal static async Task ReadAsFileAsync(this HttpContent content, string filename)
        {
            var pathname = Path.GetFullPath(filename);
            FileStream stream = null;
            try
            {
                stream = new FileStream(pathname, FileMode.Create, FileAccess.Write, FileShare.None);
                await content.CopyToAsync(stream);
                stream.Close();
            }
            catch
            {
                if (stream != null)
                {
                    stream.Close();
                }
                File.Delete(filename);
                throw;
            }
        }
        public static async Task<string> DownloadAsync(string url, string catalog, string ext)
        {
            var uid = WeiZhiDirectories.CacheFilePath(catalog, url, ext);
            if (File.Exists(uid))
                return uid;
            var fp = uid + '.' + Seed.Next();
            try
            {
                using (var client = new HttpClient())
                {
                    var resp = await client.GetAsync(url);
                    if (!resp.IsSuccessStatusCode)
                        return null;
                    await ReadAsFileAsync(resp.Content, fp);
                    if(!File.Exists(uid))
                        File.Move(fp, uid);
                    return uid;
                }
            }catch(HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            File.Delete(fp);
            return null;
        }                    
    }
}