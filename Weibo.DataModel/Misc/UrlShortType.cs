using System.ComponentModel;
using System.Runtime.Serialization;

namespace Weibo.DataModel.Misc
{
    [DataContract] //defined by sina
    public enum UrlType
    {
        [Description("网页")]
        [EnumMember]
        Normal = 0,

        [Description("视频")]
        [EnumMember]
        Video = 1,

        [Description("音乐")]
        [EnumMember]
        Music = 2,

        [Description("活动")]
        [EnumMember]
        Event = 3,

        [Description("谈话")]
        [EnumMember]
        Talk = 10,

        [Description("投票")]
        [EnumMember]
        Vote = 5,

        [Description("表情")]
        [EnumMember]
        Emotion = 13,

        [Description("新闻")]
        [EnumMember]
        News = 7,

        [Description("博客")]
        [EnumMember]
        Blog = 29,

        [Description("微吧")]
        [EnumMember]
        Bar = 17,

        [Description("媒体")]
        [EnumMember]
        Media = 27,
    }
}