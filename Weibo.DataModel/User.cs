using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class User : UserWithoutStatus
    {
        [DataMember]
        public long status_id { get; set; }

        [DataMember]
        public StatusInUser status { get; set; }
    }
}