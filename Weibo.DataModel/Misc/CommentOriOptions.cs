using System;
using System.Runtime.Serialization;

namespace Weibo.DataModel.Misc
{
    //comment_ori	int	当评论一条转发微博时，是否评论给原微博。0:不评论给原微博。1：评论给原微博。默认0
    //without_mention		1：回复中不自动加入“回复@用户名”，0：回复中自动加入“回复@用户名”.默认为0. 
    [DataContract]
    [Flags]
    public enum CommentOriOptions
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        ToOriginal = 1,

    }
}