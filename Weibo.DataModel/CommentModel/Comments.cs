using System.Runtime.Serialization;

namespace Weibo.DataModel.Comment
{
    [DataContract]
    public class Comments
    {
        [DataMember]
        public CommentModel.Comment[] comments { get; set; }

        [DataMember]
        public long previous_cursor { get; set; }

        [DataMember]
        public long next_cursor { get; set; }

        [DataMember]
        public int total_number { get; set; }
    }
}