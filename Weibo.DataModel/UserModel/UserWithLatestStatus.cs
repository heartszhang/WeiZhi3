using System.Runtime.Serialization;
using Weibo.DataModel.Status;

namespace Weibo.DataModel.UserModel
{
    [DataContract]
    public class UserWithLatestStatus : UserModel.User
    {
        [DataMember]
        public long status_id { get; set; }

        [DataMember]
        public StatusInUser status { get; set; }
    }
}