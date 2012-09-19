namespace Weibo.ViewModels.StatusRender
{
    public class Token
    {
        public Token()
        {
            text = string.Empty;
            tag = WeiboTokenTypes.Part;
        }

        public string text { get; set; }
        public char flag { get; set; }
        public WeiboTokenTypes tag { get; set; }
    }
}