using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class UrlInfos
    {
        [DataMember]
        public UrlInfo[] urls { get; set; }
    }
}