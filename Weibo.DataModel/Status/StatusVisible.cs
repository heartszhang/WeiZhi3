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
    //comment_ori	int	������һ��ת��΢��ʱ���Ƿ����۸�ԭ΢����0:�����۸�ԭ΢����1�����۸�ԭ΢����Ĭ��0
    //without_mention		1���ظ��в��Զ����롰�ظ�@�û�������0���ظ����Զ����롰�ظ�@�û�����.Ĭ��Ϊ0. 
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
    //�Ƿ���ת����ͬʱ�������ۣ�0����1�����۸���ǰ΢����2�����۸�ԭ΢����3�������ۣ�Ĭ��Ϊ0 ��
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