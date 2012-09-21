using System.Runtime.Serialization;
using Weibo.DataModel;

namespace Weibo.ViewModels.DataModels
{
    [DataContract]
    public class Account
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string AccessToken { get; set; }

        [DataMember]
        public long Expired { get; set; }

        [DataMember]
        public WeiboSourcesType Source { get; set; }
    }
}