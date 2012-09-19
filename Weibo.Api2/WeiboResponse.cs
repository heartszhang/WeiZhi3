using System.Net;

namespace Weibo.Api2
{
    public class WeiboResponse
    {
        public dynamic Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
    }
}