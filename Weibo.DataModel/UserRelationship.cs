using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class UserRelationship
    {
        [DataMember]
        public long id { get; set; }
        [DataMember]
        public string screen_name { get; set; }
        [DataMember]
        public bool following { get; set; }
        [DataMember]
        public bool followed_by { get; set; }
        [DataMember]
        public bool notifications_enabled { get; set; }
    }
}