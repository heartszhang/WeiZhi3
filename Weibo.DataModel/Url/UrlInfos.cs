using System.Runtime.Serialization;

namespace Weibo.DataModel.Url
{
    [DataContract]
    public class UrlInfos
    {
        [DataMember]
        public UrlInfo[] urls { get; set; }
    }
}