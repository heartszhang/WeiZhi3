using System;
using System.Runtime.Serialization;

namespace Weibo.DataModel.Misc
{
    //comment_ori	int	������һ��ת��΢��ʱ���Ƿ����۸�ԭ΢����0:�����۸�ԭ΢����1�����۸�ԭ΢����Ĭ��0
    //without_mention		1���ظ��в��Զ����롰�ظ�@�û�������0���ظ����Զ����롰�ظ�@�û�����.Ĭ��Ϊ0. 
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