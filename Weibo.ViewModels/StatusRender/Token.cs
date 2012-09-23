namespace Weibo.ViewModels.StatusRender
{
    public class Token
    {
        public Token()
        {
            text = string.Empty;
            tag = TokenTypes.Part;
        }

        public string text { get; set; }
        public char flag { get; set; }
        public TokenTypes tag { get; set; }
    }
}