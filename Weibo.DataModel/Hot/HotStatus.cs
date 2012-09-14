using System.Runtime.Serialization;
using Weibo.DataModel.Status;

namespace Weibo.DataModel.Hot
{
    [DataContract]
    public class HotStatus
    {
        [DataMember]
        public StatusWithUser status { get; set; }
        [DataMember]
        public string pid { get; set; }
        [DataMember]
        public int pwidth { get; set; }
        [DataMember]
        public int pheight { get; set; }
    }
}