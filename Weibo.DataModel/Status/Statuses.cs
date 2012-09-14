using System.Runtime.Serialization;

namespace Weibo.DataModel.Status
{
    [DataContract]
    public class Statuses
    {
        [DataMember]
        public StatusWithUser[] statuses { get; set; }
        [DataMember]
        public long previous_cursor { get; set; }
        [DataMember]
        public long next_cursor { get; set; }
        [DataMember]
        public int total_number { get; set; }
    }
}