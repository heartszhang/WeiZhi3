using System.Runtime.Serialization;
using Weibo.DataModel.Misc;

namespace Weibo.DataModel.Remind
{
    [DataContract]
    public class RemindUnreadCount
    {
        public RemindUnreadCount() {}

        public RemindUnreadCount(UnreadInfo ui)
        {
            cmt = ui.comments;
            dm = ui.dm;
            follower = ui.followers;
            mention_status = ui.mentions;
            status = ui.status;
        }

        public RemindUnreadCount(RemindUnreadCountV1 ruc)
        {
            status = ruc.feed;
            follower = ruc.attention;
            cmt = ruc.comment;
            dm = ruc.msg;
            mention_status = ruc.atme;
            mention_cmt = ruc.atcmt;
            group = ruc.group;
            notice = ruc.notice;
            invite = ruc.invite;
            badge = ruc.badge;
            photo = ruc.photo;
        }

        //��΢��δ����
        [DataMember]
        public int status { get; set; }

        [DataMember]
        public int follower { get; set; }

        //��������
        [DataMember]
        public int cmt { get; set; }

        //��˽����
        [DataMember]
        public int dm { get; set; }

        //���ἰ�ҵ�΢����

        [DataMember]
        public int mention_status { get; set; }

        //���ἰ�ҵ�������
        [DataMember]
        public int mention_cmt { get; set; }

        //΢Ⱥ��Ϣδ����
        [DataMember]
        public int group { get; set; }

        //��֪ͨδ����
        [DataMember]
        public int notice { get; set; }

        //������δ����
        [DataMember]
        public int invite { get; set; }

        //��ѫ����
        [DataMember]
        public int badge { get; set; }

        //�����Ϣδ����
        [DataMember]
        public int photo { get; set; }

        [DataMember]
        public int private_group { get; set; }

        //for remind
        [DataMember]
        public CommentModel.Comment[] latest_comments { get; set; }

        [DataMember]
        public CommentModel.Comment[] mention_comments { get; set; }

    }
}