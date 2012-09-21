using System.Net;

namespace Weibo.Apis.Net
{
    public class RestResultBase
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Reason { get; set; }
        internal RestResultBase()
        {
            StatusCode = HttpStatusCode.Unused;
            Reason = "Unknown";
        }
        internal RestResultBase(HttpStatusCode scode, string err)
        {
            StatusCode = scode;
            Reason = err;
        }
        internal RestResultBase(RestResultBase rhs)
        {
            StatusCode = rhs.StatusCode;
            Reason = rhs.Reason;
        }
        public bool Failed()
        {
            return StatusCode != HttpStatusCode.OK;
        }
        public int Error()
        {
            return (int)StatusCode;
        }
    }
}