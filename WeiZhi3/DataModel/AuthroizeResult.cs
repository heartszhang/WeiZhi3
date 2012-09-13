namespace WeiZhi3.DataModel
{
    internal struct AuthroizeResult
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