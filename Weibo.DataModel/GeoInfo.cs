using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class GeoInfo
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
        public string address { get; set; }
    }
}