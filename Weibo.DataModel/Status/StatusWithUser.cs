using System.Runtime.Serialization;

namespace Weibo.DataModel.Status
{
    [DataContract]
    public class StatusWithUser : StatusUserBase
    {
        [DataMember]
        public StatusUserBase retweeted_status { get; set; }
    }
}