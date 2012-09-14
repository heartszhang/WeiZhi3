using System.Runtime.Serialization;

namespace Weibo.DataModel.UserModel
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