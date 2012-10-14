using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class Geo
    {//{"type": "Point","coordinates": [24.459173, 118.080399]}
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public double[] coordinates { get; set; }
    }
    [DataContract]
    public class GeoAddress
    {
        [DataMember]
        public double longitude { get; set; }
        [DataMember]
        public double latitude { get; set; }
        [DataMember]
        public int city { get; set; }
        [DataMember]
        public int province { get; set; }
        [DataMember]
        public string city_name { get; set; }
        [DataMember]
        public string province_name { get; set; }
        [DataMember]
        public string district_name { get; set; }
        [DataMember]
        public string address { get; set; }
    }
    [DataContract]
    public class GeoAddresses
    {
        [DataMember]
        public GeoAddress[] geos { get; set; }
    }
}