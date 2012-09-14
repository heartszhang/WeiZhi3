using System.Runtime.Serialization;
using Weibo.DataModel.UserModel;

namespace Weibo.DataModel.Status
{
    [DataContract]
    public class StatusUserBase : StatusBaseBase
    {
        [DataMember]
        public User user { get; set; }
    }
}