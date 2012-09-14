namespace Weibo.Apis.StatusRender
{
    public class Token
    {
        public Token()
        {
            text = "";
            tag = WeiboTextTokenTypes.Normal;
        }

        public string text { get; set; }
        //        public string name { get; set; }
        public WeiboTextTokenTypes tag { get; set; }
    }
}