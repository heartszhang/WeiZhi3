using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class FlowDocumentInfo
    {
        [DataMember]
        public string url_short { get; set; }
        [DataMember]
        public int length { get; set; }
        [DataMember]
        public string url_long { get; set; }
        [DataMember]
        public string local_file_path { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string keywords { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string widget { get; set; }

        [DataMember]
        public string thumbnail { get; set; }//local-image.png
        [DataMember]
        public string image { get; set; }//content image big enough
    }
}