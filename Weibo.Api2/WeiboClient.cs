using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Weibo.Api2
{
    internal class WeiboClient
    {
        internal static async Task<WeiboResponse> WeiboGet(WeiboRequestHandler handler, WeiboSources src)
        {
            var url = handler.CreateUrl(src);
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
                        if (resp.IsSuccessStatusCode)
                            rtn.Result = JToken.Parse(await resp.Content.ReadAsStringAsync());
                        break;
                    }
                }
                catch (HttpRequestException e)
                {//don't retry any request exception
                    rtn.StatusCode = HttpStatusCode.Unused;
                    rtn.ReasonPhrase = e.Message;
                }
            }
            return rtn;
        }          
    }
    
}


