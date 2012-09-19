namespace Weibo.ViewModels.StatusRender
{
    public enum WeiboTokenTypes
    {
        Reserved = 0,
        Part,
        Punctuation,//ends with ,.?!and some punctuations
        Quote,//"quote" 《，
        Hyperlink , //http://t.cn/xxx
        Name , // @name
        CopyedFrom , // //@xxx:
        Topic , // #xxx#
        Emotion, // [xxx],【，L
        End ,//last sentence ,with or without punctuation
        ReplyTo ,//回复Name:        
    }
}