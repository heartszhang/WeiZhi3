using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class StatusVisible
    {
        [DataMember]
        public int type { get; set; }
        [DataMember]
        public int list_id { get; set; }
    }
}