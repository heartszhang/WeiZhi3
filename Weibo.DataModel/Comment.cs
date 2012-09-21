using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class Comment
    {
        [DataMember]
        public string created_at { get; set; }

        [DataMember]
        public long id { get; set; }

        [DataMember]
        public string text { get; set; }

        [DataMember]
        public User user { get; set; }

        [DataMember]
        public Status status { get; set; }

        [DataMember]
        public string source { get; set; }

        [DataMember]
        public bool favorited { get; set; }

        [DataMember]
        public bool truncated { get; set; }

        [DataMember]
        public Comment reply_comment { get; set; }

        [DataMember]
        public long mid { get; set; }
    }
}