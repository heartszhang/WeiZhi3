using System;
using System.Runtime.Serialization;

namespace Weibo.DataModel.Misc
{
    //是否在转发的同时发表评论，0：否、1：评论给当前微博、2：评论给原微博、3：都评论，默认为0 。
    [DataContract]
    [Flags]
    public enum RetweetOptions
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Comment = 1,
        [EnumMember]
        CommentToRetweet = 2,
        [EnumMember]
        All = Comment | CommentToRetweet,
    }
}