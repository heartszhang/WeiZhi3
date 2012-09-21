using System.Runtime.Serialization;

namespace Weibo.DataModel.Misc
{
    [DataContract] //defined by sina
    public enum UrlShortType
    {
        [EnumMember]
        Normal = 0,
        [EnumMember]
        Video = 1,
        [EnumMember]
        Music = 2,
        [EnumMember]
        Event = 3,
        [EnumMember]
        Talk = 10,
        [EnumMember]
        Vote = 5,
        [EnumMember]
        Emotion = 13,
        [EnumMember]
        BlogNews = 7,
    }
}