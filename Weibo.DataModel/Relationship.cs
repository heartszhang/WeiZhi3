using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class Relationship
    {
        [DataMember]
        public UserRelationship source { get; set; }
        [DataMember]
        public UserRelationship target { get; set; }
    }
}