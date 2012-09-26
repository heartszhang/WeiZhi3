using System.ComponentModel;
using System.Runtime.Serialization;

namespace Weibo.DataModel.Misc
{
    [DataContract] //defined by sina
    public enum UrlType
    {
        [Description("��ҳ")]
        [EnumMember]
        Normal = 0,

        [Description("��Ƶ")]
        [EnumMember]
        Video = 1,

        [Description("����")]
        [EnumMember]
        Music = 2,

        [Description("�")]
        [EnumMember]
        Event = 3,

        [Description("̸��")]
        [EnumMember]
        Talk = 10,

        [Description("ͶƱ")]
        [EnumMember]
        Vote = 5,

        [Description("����")]
        [EnumMember]
        Emotion = 13,

        [Description("����")]
        [EnumMember]
        News = 7,

        [Description("����")]
        [EnumMember]
        Blog = 29,

        [Description("΢��")]
        [EnumMember]
        Bar = 17,

        [Description("ý��")]
        [EnumMember]
        Media = 27,
    }
}