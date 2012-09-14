using System.Runtime.Serialization;
using Weibo.DataModel.Status;

namespace Weibo.DataModel.Misc
{
    [DataContract]
    public class SearchStatuses
    {
        [DataMember]
        public StatusWithUser[] results { get; set; }
        [DataMember]
        public long total_count_maybe { get; set; }
    }
}