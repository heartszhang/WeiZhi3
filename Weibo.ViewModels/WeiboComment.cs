using System;
using Weibo.DataModel;

namespace Weibo.ViewModels
{
    public class WeiboComment : ObservableObjectExt
    {
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
        }
    }
}