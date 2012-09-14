namespace Weibo.Apis.StatusRender
{
    public enum WeiboTextTokenTypes
    {
        Reserved = 0,
        Normal = 1,
        Hyperlink = 2, //http://t.cn/xxx
        Name = 4, // @name
        SharedFrom = 8, // //@xxx:
        Topic = 16, // #xxx#
        KeyValue = 128, // 
        ScreenName = 32,
        Footnote = 64,
        Emotion = 512, // [xxx]
        Writer = 256,
        SentenseEnd = 1024,
        Punctuations = 2048,
        FirstSentence = 4096,
        ReplyTo = 8192,//回复Name:
        FirstSentenceRemoved = ReplyTo * 2,
        Quote = FirstSentenceRemoved *2,
        NameLength = 32
    }
}