using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class HotStatus
    {
        [DataMember]
        public Status status { get; set; }
        [DataMember]
        public string pid { get; set; }
        [DataMember]
        public int pwidth { get; set; }
        [DataMember]
        public int pheight { get; set; }
    }
}