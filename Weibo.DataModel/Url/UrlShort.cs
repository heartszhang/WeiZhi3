using System.Runtime.Serialization;

namespace Weibo.DataModel.Url
{
    [DataContract]
    public class UrlShort
    {
        [DataMember]
        public string url_short { get; set; }

        [DataMember]
        public string url_long { get; set; }

        [DataMember]
        public int type { get; set; }

        //0:normal, 1: video
    }

}