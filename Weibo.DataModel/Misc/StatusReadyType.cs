using System.Runtime.Serialization;

namespace Weibo.DataModel.Misc
{
    [DataContract]
    public enum StatusFeature
    {
        [EnumMember]
        All = 0,
        [EnumMember]
        Original = 1,
        [EnumMember]
        Picture = 2,
        [EnumMember]
        Video = 3,
        [EnumMember]
        Music = 4,
    }
}