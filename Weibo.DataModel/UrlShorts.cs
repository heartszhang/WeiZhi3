using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class UrlShorts
    {
        [DataMember]
        public UrlShort[] urls { get; set; }
    }
}