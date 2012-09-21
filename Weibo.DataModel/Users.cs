using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class Users
    {
        [DataMember]
        public User[] users { get; set; }

        [DataMember]
        public int next_cursor { get; set; }

        [DataMember]
        public int previous_cursor { get; set; }

        [DataMember]
        public int total_number { get; set; }
    }
}