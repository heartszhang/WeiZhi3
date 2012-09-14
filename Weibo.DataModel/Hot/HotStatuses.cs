using System.Runtime.Serialization;

namespace Weibo.DataModel.Hot
{
    [DataContract]
    public class HotStatuses
    {
        [DataMember]
        public HotStatus[] statuses { get; set; }
        [DataMember]
        public int total_number { get; set; }
    }
}