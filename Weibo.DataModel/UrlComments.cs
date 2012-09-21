using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class UrlComments
    {
        [DataMember]
        public Comment[] share_comments { get; set; }

        [DataMember]
        public string url_short { get; set; }
    }
}