using System.Runtime.Serialization;
using Weibo.DataModel.Misc;

namespace Weibo.DataModel
{
    [DataContract]
    public class UrlInfo
    {
        [DataMember]
        public string url_short { get; set; }
        [DataMember]
        public string url_long { get; set; }
        [DataMember]
        public UrlType type { get; set; }
        [DataMember]
        public bool result { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string last_modified { get; set; }
        //annotations
    }
}