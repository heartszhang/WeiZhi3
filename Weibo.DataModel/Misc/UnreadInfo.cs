using System.Runtime.Serialization;

namespace Weibo.DataModel.Misc
{
    [DataContract]
    public class UnreadInfo
    {
        [DataMember]
        public int comments { get; set; }

        [DataMember]
        public int followers { get; set; }

        [DataMember]
        public int new_status { get; set; }

        [DataMember]
        public int dm { get; set; }

        [DataMember]
        public int mentions { get; set; }

        [DataMember]
        public int status { get; set; }
    }
}