using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Weibo.Apis.Net
{
    public class DoanloadFileResult
    {
        public string file_path { get; set; }
        public string content_encoding { get; set; }
    }
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
        public static async Task<string> DownloadAsync(string url, string catalog, string ext,string mediatype , long maxlength )
        {
            var uid = WeiZhiDirectories.CacheFilePath(catalog, url, ext);
            if (File.Exists(uid))
                return uid;
            var fp = uid + '.' + Seed.Next();
            try
            {
                using (var client = new HttpClient(new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                }))
                {
                    client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
                    client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("deflate"));
                    var resp = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    if (!resp.IsSuccessStatusCode)
                    {
                        return null;
                    }
                    if (!string.IsNullOrEmpty(mediatype) && resp.Content.Headers.ContentType.MediaType != mediatype)
                    {
                        return null;
                    }
                    if (resp.Content.Headers.ContentLength > maxlength)
                    {
                        return null;
                    }
                    Debug.WriteLine("{0} - {1} ; {2}",url,resp.Content.Headers.ContentType.CharSet
                        , resp.Content.Headers.ContentType.MediaType);
                    var charset = resp.Content.Headers.ContentType.CharSet;
                    await ReadAsFileAsync(resp.Content, fp);
                    if (!File.Exists(uid))
                        File.Move(fp, uid);
                    File.WriteAllText(uid +".enc", charset);
                    return uid;
                }
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message, e.InnerException.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            File.Delete(fp);
            return null;
        }

        public static async Task<string> DownloadAsync(string url, string catalog, string ext)
        {
            var uid = WeiZhiDirectories.CacheFilePath(catalog, url, ext);
            if (File.Exists(uid))
                return uid;
            var fp = uid + '.' + Seed.Next();
            try
            {
                using (var client = new HttpClient(new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                }))
                {
                    client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
                    client.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("deflate")); 
                    var resp = await client.GetAsync(url);
                    if (!resp.IsSuccessStatusCode)
                        return null;
                    await ReadAsFileAsync(resp.Content, fp);
                    if (!File.Exists(uid))
                        File.Move(fp, uid);
                    return uid;
                }
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message, e.InnerException.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            File.Delete(fp);
            return null;
        }                    
    }
}