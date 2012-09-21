using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class SearchStatuses
    {
        [DataMember]
        public Status[] results { get; set; }
        [DataMember]
        public long total_count_maybe { get; set; }
    }
}