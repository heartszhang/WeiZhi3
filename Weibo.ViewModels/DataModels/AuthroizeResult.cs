namespace Weibo.ViewModels.DataModels
{
    public struct AuthroizeResult
    {
        public string AccessToken;
        public long RemindIn;
        public long ExpiresIn;
        public int ErrorCode;
        public string Error;
        public string ErrorDescription;
        public long Id;
    }
}