using System.Runtime.Serialization;
using Weibo.DataModel.Misc;

namespace Weibo.DataModel
{
    [DataContract]
    public class StatusProfile : StatusWithoutUser
    {
        [DataMember]
        public StatusWithoutRt retweeted_status { get; set; }
    }
}