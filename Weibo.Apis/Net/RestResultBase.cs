namespace Weibo.Apis.Net
{
    public class RestResultBase
    {
        public int StatusCode;
        public string Error;
        internal RestResultBase()
        {
            StatusCode = 200;
            Error = "OK";
        }
        internal RestResultBase(int scode, string err)
        {
            StatusCode = scode;
            Error = err;
        }
        internal RestResultBase(RestResultBase rhs)
        {
            StatusCode = rhs.StatusCode;
            Error = rhs.Error;
        }
    }
}