using System;
using System.Runtime.Serialization;

namespace Weibo.DataModel.Status
{
    [DataContract]
    public class StatusVisible
    {
        [DataMember]
        public int type { get; set; }
        [DataMember]
        public int list_id { get; set; }
    }
    //comment_ori	int	当评论一条转发微博时，是否评论给原微博。0:不评论给原微博。1：评论给原微博。默认0
    //without_mention		1：回复中不自动加入“回复@用户名”，0：回复中自动加入“回复@用户名”.默认为0. 
    [DataContract][Flags]
    public enum CommentOriOptions
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        ToOriginal = 1,

    }
    [DataContract][Flags]
    public enum WithoutMentionOptions
    {
        [EnumMember]
        WithMention = 0,
        [EnumMember]
        WithoutMention = 1,
    }
    //是否在转发的同时发表评论，0：否、1：评论给当前微博、2：评论给原微博、3：都评论，默认为0 。
    [DataContract][Flags]
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