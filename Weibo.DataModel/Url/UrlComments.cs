using System.Runtime.Serialization;

namespace Weibo.DataModel.Url
{
    [DataContract]
    public class UrlComments
    {
        [DataMember]
        public CommentModel.Comment[] share_comments { get; set; }

        [DataMember]
        public string url_short { get; set; }
    }
}