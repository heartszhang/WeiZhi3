using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class Reposts
    {
        [DataMember]
        public Status[] reposts { get; set; }
        [DataMember]
        public long previous_cursor { get; set; }
        [DataMember]
        public long next_cursor { get; set; }
        [DataMember]
        public int total_number { get; set; }
    }
}