using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class Geoes
    {
        [DataMember]
        public GeoInfo[] geos { get; set; }
    }
}