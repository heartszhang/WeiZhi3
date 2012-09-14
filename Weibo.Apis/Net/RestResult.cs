namespace Weibo.Apis.Net
{
    public class RestResult<TResult> : RestResultBase
    {
        public RestResult()
        {
        }

        public RestResult(RestResultBase rhs):base(rhs)
        {
            
        }
        public TResult Value;
    }
}