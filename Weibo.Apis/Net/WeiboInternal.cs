using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weibo.DataModel;

namespace Weibo.Apis.Net
{
    public class WeiboInternal
    {
        internal static async Task<RestResult<TResult>>
            HttpsPost<TResult>(string path, NameValueCollection parameters) where TResult : class
        {
            var rtn = new RestResult<TResult>();
            using (var client = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                foreach (var k in parameters.AllKeys)
                {
                    form.Add(new StringContent(parameters[k]), k);
                }

                var resp = await client.PostAsync(path, form);
                rtn.StatusCode = resp.StatusCode;
                rtn.Reason = resp.ReasonPhrase;
                if (resp.IsSuccessStatusCode)
                    rtn.Value = JsonConvert.DeserializeObject<TResult>(await resp.Content.ReadAsStringAsync());
                else if ((int) resp.StatusCode >= 400)
                {
                    var er = JsonConvert.DeserializeObject<WeiboError>(await resp.Content.ReadAsStringAsync());
                    rtn.Reason = er.Translate();
                }
            }
            return rtn;
        }

        internal static async Task<RestResult<TResult>> HttpsGet<TResult>(string url)
            where TResult : class
        {
            Debug.WriteLine(url);
            var rtn = new RestResult<TResult>();
            using (var client = new HttpClient())
            {
                for (var r = 0; r < 2; ++r)
                {
                    try
                    {
                        var resp = await client.GetAsync(url);
                        rtn.StatusCode = resp.StatusCode;
                        rtn.Reason = resp.ReasonPhrase;

                        if ((int) resp.StatusCode >= 500) //server error
                            continue; //retry
                        if (resp.IsSuccessStatusCode)
                            rtn.Value = JsonConvert.DeserializeObject<TResult>(await resp.Content.ReadAsStringAsync());
                        else if ((int) resp.StatusCode >= 400)
                        {
                            var er = JsonConvert.DeserializeObject<WeiboError>(await resp.Content.ReadAsStringAsync());
                            rtn.Reason = er.Translate();
                        }
                        //JToken.Parse(await resp.Content.ReadAsStringAsync());
                        break;
                    }
                    catch (HttpRequestException e)
                    {
//don't retry any request exception
                        rtn.StatusCode = HttpStatusCode.Unused;
                        rtn.Reason = e.Message;
                        Debug.WriteLine(e.Message);
                    }
                }
            }
            return rtn;
        }
    }
}