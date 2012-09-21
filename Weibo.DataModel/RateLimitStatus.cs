using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class RateLimitStatus
    {
        [DataMember]
        public int ip_limit { get; set; }
        [DataMember]
        public string limit_time_unit { get; set; } //"HOURS",
        [DataMember]
        public int remaining_ip_hits { get; set; }
        [DataMember]
        public int remaining_user_hits { get; set; }
        [DataMember]
        public string reset_time { get; set; }
        [DataMember]
        public int reset_time_in_seconds { get; set; }
        [DataMember]
        public int user_limit { get; set; }
    }
}