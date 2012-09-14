using System.Runtime.Serialization;

namespace Weibo.DataModel.Remind
{
    [DataContract]
    public class RemindUnreadCountV1
    {
        [DataMember]
        public int feed { get; set; }

        [DataMember]
        public int attention { get; set; }

        [DataMember]
        public int comment { get; set; }

        [DataMember]
        public int msg { get; set; }

        [DataMember]
        public int atme { get; set; }

        [DataMember]
        public int atcmt { get; set; }

        [DataMember]
        public int group { get; set; }

        [DataMember]
        public int notice { get; set; }

        [DataMember]
        public int invite { get; set; }

        [DataMember]
        public int badge { get; set; }

        [DataMember]
        public int photo { get; set; }
    }
}