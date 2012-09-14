using System.Runtime.Serialization;

namespace Weibo.DataModel.Status
{
    [DataContract]
    public class StatusProfile : StatusBaseBase
    {
        [DataMember]
        public StatusUserBase retweeted_status { get; set; }
    }
}