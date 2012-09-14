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

        //新微博未读数
        [DataMember]
        public int status { get; set; }

        [DataMember]
        public int follower { get; set; }

        //新评论数
        [DataMember]
        public int cmt { get; set; }

        //新私信数
        [DataMember]
        public int dm { get; set; }

        //新提及我的微博数

        [DataMember]
        public int mention_status { get; set; }

        //新提及我的评论数
        [DataMember]
        public int mention_cmt { get; set; }

        //微群消息未读数
        [DataMember]
        public int group { get; set; }

        //新通知未读数
        [DataMember]
        public int notice { get; set; }

        //新邀请未读数
        [DataMember]
        public int invite { get; set; }

        //新勋章数
        [DataMember]
        public int badge { get; set; }

        //相册消息未读数
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