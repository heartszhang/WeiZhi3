using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Weibo.Api2.Sina;
using Weibo.DataModel;

namespace Weibo.Api2
{
    internal class WeiboClientInternal
    {
        internal static async Task<WeiboResponseGeneric<TResult>> WeiboGet<TResult>(WeiboRequestHandler handler, WeiboSourcesType src = WeiboSourcesType.Sina)
        {
            var url = handler.CreateUrl(src);
            Debug.WriteLine(url);
            var rtn = new WeiboResponseGeneric<TResult>();

            using (var client = new HttpClient())
            {
                try
                {
                    for (var r = 0; r < handler.RetryTimes; ++r)
                    {
                        var resp = await client.GetAsync(url);
                        rtn.StatusCode = resp.StatusCode;
                        rtn.ReasonPhrase = resp.ReasonPhrase;

                        if ((int)resp.StatusCode >= 500)//server error
                            continue;//retry
                        if (resp.IsSuccessStatusCode)
                            rtn.Result = JsonConvert.DeserializeObject<TResult>(await resp.Content.ReadAsStringAsync());
                        else if ((int)resp.StatusCode >= 400)
                        {
                            var er = JsonConvert.DeserializeObject<WeiboError>(await resp.Content.ReadAsStringAsync());
                            rtn.ReasonPhrase = er.Translate();
                        }
                        //JToken.Parse(await resp.Content.ReadAsStringAsync());
                        break;
                    }
                }
                catch (HttpRequestException e)
                {//don't retry any request exception
                    rtn.StatusCode = HttpStatusCode.Unused;
                    rtn.ReasonPhrase = e.Message;
                    Debug.WriteLine(e.Message);
                }
            }
            return rtn;
        }
        internal static async Task<WeiboResponse> WeiboGet(WeiboRequestHandler handler, WeiboSourcesType src = WeiboSourcesType.Sina)
        {
            var url = handler.CreateUrl(src);
            Debug.WriteLine(url);
            var rtn = new WeiboResponse();

            using (var client = new HttpClient())
            {
                try
                {
                    for (var r = 0; r < handler.RetryTimes; ++r)
                    {
                        var resp = await client.GetAsync(url);
                        rtn.StatusCode = resp.StatusCode;
                        rtn.ReasonPhrase = resp.ReasonPhrase;

                        if ((int)resp.StatusCode >= 500)//server error
                            continue;//retry
                        if (resp.IsSuccessStatusCode || (int)resp.StatusCode>= 400)
                            rtn.Result = JToken.Parse(await resp.Content.ReadAsStringAsync());
                        break;
                    }
                }
                catch (HttpRequestException e)
                {//don't retry any request exception
                    rtn.StatusCode = HttpStatusCode.Unused;
                    rtn.ReasonPhrase = e.Message;
                    Debug.WriteLine(e.Message);
                }
            }
            return rtn;
        }          
    }
    
}


