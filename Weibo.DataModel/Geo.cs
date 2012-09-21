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
}