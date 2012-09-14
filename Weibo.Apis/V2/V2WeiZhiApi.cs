using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel.Status;

namespace Weibo.Apis.V2
{
    public partial class Weibo
    {
        protected const string AuthorityPathV2 = "https://api.weibo.com/2";

        protected static string GetRequestAddress(string path, string baseuri = null)
        {
            return  (baseuri ?? AuthorityPathV2)+ '/' + path;
        }

        protected static async Task<RestResult<TResult>> 
            UploadAsync<TResult>(string address, NameValueCollection data, string filepath, string name)
            where TResult : class
        {
            var boundary = Environment.TickCount.ToString("x");
            using (var client = new WebClient2())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "multipart/form-data; boundary=" + boundary);

                var sb = new StringBuilder();
                const string parameterData = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
                foreach (var nv in data.Keys)
                {
                    sb.AppendFormat("--{0}\r\n", boundary);
                    sb.AppendFormat(parameterData, nv,Uri.EscapeDataString( data[nv.ToString()]));
                }

                const string fileData =
                    "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                sb.AppendFormat("--{0}\r\n", boundary);
                sb.AppendFormat(fileData, name, "atta.jpg");
                var buf = Encoding.ASCII.GetBytes(sb.ToString());
                var fbuf = File.ReadAllBytes(filepath);

                var endline = Encoding.ASCII.GetBytes(string.Format("\r\n--{0}--", boundary));
                byte[] updata;
                using (var stream = new MemoryStream())
                {
                    stream.Write(buf, 0, buf.Length);

                    stream.Write(fbuf, 0, fbuf.Length);
                    stream.Write(endline, 0, endline.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    updata = stream.ToArray();
                }

                var rtn = new RestResult<TResult>();
                var retry = 2;
                do
                {
                    try
                    {
                        var resp = await client.UploadDataTaskAsync(address, "POST", updata);
                        rtn.Value = ResponseParser.Extract<TResult>(resp);
                        break;
                    }
                    catch (Exception e)
                    {
                        SetResultFromException(e, rtn);
                    }

                } while (--retry > 0);
                return rtn;
            }
        }

        protected static async Task<RestResult<TResult>>
            HttpsPost<TResult>(string path, NameValueCollection parameters, string baseuri = null) where TResult : class
        {
            using (var wc = new WebClient2 { Encoding = Encoding.UTF8 })
            {
                var rtn = new RestResult<TResult>();

                var retry = 2;
                do
                {
                    try
                    {
                        var r = await wc.UploadValuesTaskAsync(GetRequestAddress(path), parameters);
                        rtn.Value = ResponseParser.Extract<TResult>(r);
                        break;
                    }
                    catch (Exception e)
                    {
                        SetResultFromException(e, rtn);
                    }
                } while (--retry > 0);

                return rtn;
            }
        }

        static void SetResultFromException(WebException e, RestResultBase result)
        {
            if (e.Status != WebExceptionStatus.ProtocolError) return;
            var hr = (HttpWebResponse)e.Response;
            result.StatusCode = (int)hr.StatusCode;
            result.Error = hr.StatusDescription;
            var rs = hr.GetResponseStream();
            if (rs == null)
                return;
            using(var sr = new StreamReader(rs,Encoding.UTF8 ))
            {
                result.Error = sr.ReadToEnd();
                //var se = ResponseParser.Extract<SinaError>(result.Error);
               // result.Error = result.Error;
                //result.StatusCode = se.error_code;
            }
            
        }
        static void SetResultFromException(Exception e, RestResultBase result)
        {
            result.StatusCode = (int)HttpStatusCode.Unused;
            result.Error = e.Message;
            var web_exception = e as WebException;
            if (web_exception != null) 
                SetResultFromException(web_exception, result);
        }
        protected static async Task<RestResult<TResult>> HttpsGet<TResult>(string path, string baseuri = null)
            where TResult: class
        {
            using (var wc = new WebClient2())
            {
                var rtn = new RestResult<TResult>();
                var retry = 2;
                do
                {
                    try
                    {
                        var resp = await wc.DownloadDataTaskAsync(GetRequestAddress(path, baseuri));
                        rtn.Value = ResponseParser.Extract<TResult>(resp);
                        break;
                    }
                    catch (Exception e)
                    {
                        SetResultFromException(e, rtn);
                        Console.WriteLine(path);
                        Console.WriteLine(e.Message);
                    }
                } while (--retry > 0); 

                return rtn;
            }
        }
    }
}