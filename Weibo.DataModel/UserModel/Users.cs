using System.Runtime.Serialization;

namespace Weibo.DataModel.UserModel
{
    [DataContract]
    public class Users
    {
        [DataMember]
        public UserWithLatestStatus[] users { get; set; }

        [DataMember]
        public int next_cursor { get; set; }

        [DataMember]
        public int previous_cursor { get; set; }

        [DataMember]
        public int total_number { get; set; }
    }
}