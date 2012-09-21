using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class StatusCount
    {
        [DataMember]
        public long id { get; set; }
        [DataMember]
        public int comments { get; set; }
        [DataMember]
        public int rt { get; set; }
    }
}