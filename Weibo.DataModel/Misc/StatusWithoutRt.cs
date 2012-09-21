using System.Runtime.Serialization;

namespace Weibo.DataModel.Misc
{
    [DataContract]
    public class StatusWithoutRt : StatusWithoutUser
    {
        [DataMember]
        public UserWithoutStatus user { get; set; }
    }
}