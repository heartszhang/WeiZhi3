using System.Net;

namespace Weibo.Api2
{
    public class WeiboResponse
    {
        internal dynamic Result { get; set; }
        internal HttpStatusCode StatusCode { get; set; }
        internal string ReasonPhrase { get; set; }
    }
}