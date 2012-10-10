using System;
using System.Text.RegularExpressions;
using Weibo.DataModel;

namespace Weibo.ViewModels
{
    public class WeiboComment : ObservableObjectExt
    {
        private CommentReply _replier;
        public UserExt user { get; set; }
        public DateTime created_at { get; set; }
        public string text { get; set; }
        public long id { get; set; }
        public long mid { get; set; }
        public bool favorited { get; set; }
        public bool truncated { get; set; }
        public string source { get; set; }

        public Status status { get; set; }
        public Comment reply_comment { get; set; }

        public string references { get; set; }
        public CommentReply replier { get { return _replier; } set { Set(ref _replier, value); } }
        public void assign_sina(Comment data)
        {
            created_at = WeiboStatus.time(data.created_at);
            id = data.id;
            text = data.text;
            user = new UserExt();
            if (data.user != null)
                user.assign_sina(data.user);
            status = data.status;
            source = data.source;
            favorited = data.favorited;
            truncated = data.truncated;

            reply_comment = data.reply_comment;
            mid = data.mid;

            const string pattern = @"^\s*回复@[\w]+[:：]\s*";
            text = Regex.Replace(text, pattern, string.Empty);
            const string pattern2 = @"\s*//@[\w]+[:：].*$";
            text = Regex.Replace(text, pattern2,"//...");

            if (reply_comment != null)
                references = string.Format("回复 {0} 的评论：{1}",reply_comment.user.screen_name,reply_comment.text);
            else if (status != null)
                references = string.Format("回复 {0} 的微博：{1}",status.user.screen_name,status.text);
            references = Regex.Replace(references, pattern2, string.Empty);
        }
    }
}