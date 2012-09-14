using System.Runtime.Serialization;

namespace Weibo.DataModel.Url
{
    [DataContract]
    public class UrlShorts
    {
        [DataMember]
        public UrlShort[] urls { get; set; }
    }
}