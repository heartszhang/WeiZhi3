using System.Runtime.Serialization;

namespace Weibo.DataModel.UserModel
{
    [DataContract]
    public class UserIds
    {
        [DataMember]
        public long[] ids { get; set; }

        [DataMember]
        public long previous_cursor { get; set; }

        [DataMember]
        public long next_cursor { get; set; }

        [DataMember]
        public int total_number { get; set; }
    }
}