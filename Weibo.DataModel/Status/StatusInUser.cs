using System.Runtime.Serialization;

namespace Weibo.DataModel.Status
{
    [DataContract]
    public class StatusInUser : StatusBaseBase
    {
        [DataMember]
        public StatusUserBase retweeted_status { get; set; }
    }
}