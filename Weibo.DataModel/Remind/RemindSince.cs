using System.Runtime.Serialization;

namespace Weibo.DataModel.Remind
{
    [DataContract]
    public class RemindSince
    {
        [DataMember]
        public long friends_timeline_since { get; set; }

        [DataMember]
        public long comments_to_me_since { get; set; }

        [DataMember]
        public long comments_mentions_me_since { get; set; }

        [DataMember]
        public long status_mentions_me_since { get; set; }

        [DataMember]
        public int followers_count { get; set; }
    }
}