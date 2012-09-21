using System;
using System.Runtime.Serialization;

namespace Weibo.DataModel.Misc
{
    //�Ƿ���ת����ͬʱ�������ۣ�0����1�����۸���ǰ΢����2�����۸�ԭ΢����3�������ۣ�Ĭ��Ϊ0 ��
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