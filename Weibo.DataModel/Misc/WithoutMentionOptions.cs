using System;
using System.Runtime.Serialization;

namespace Weibo.DataModel.Misc
{
    [DataContract][Flags]
    public enum WithoutMentionOptions
    {
        [EnumMember]
        WithMention = 0,
        [EnumMember]
        WithoutMention = 1,
    }
}