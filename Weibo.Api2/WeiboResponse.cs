using System.Net;

namespace Weibo.Api2
{
    public class WeiboResponse
    {
        public dynamic Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public bool Failed()
        {
            return StatusCode != HttpStatusCode.OK;
        }
        public string Reason()
        {
            var rtn = ReasonPhrase;
            if(Result != null)
            {
                rtn = Result.error;
            }
            return rtn;
        }
        public int Error()
        {
            var rtn = (int) StatusCode;
            if (Result != null)
                rtn = Result.error_code;
            return rtn;
        }
    }
    public class WeiboResponseGeneric<TResult>
    {
        public TResult Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public bool Failed()
        {
            return StatusCode != HttpStatusCode.OK;
        }
        public string Reason()
        {
            var rtn = ReasonPhrase;
            return rtn;
        }
        public int Error()
        {
            var rtn = (int)StatusCode;
            return rtn;
        }
    }    
}