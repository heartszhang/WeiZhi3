using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class UrlInfoAnnotations
    {
        #region music
        [DataMember]
        public string author { get; set; }
        [DataMember]
        public string album { get; set; }
        [DataMember]
        public string appkey { get; set; }
        [DataMember]
        public int type { get; set; }
        [DataMember]
        public string mp4 { get; set; }

        #endregion
        #region video

        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string pic { get; set; }
        [DataMember]
        public int from { get; set; }
        [DataMember]
        public string url { get; set; }
        #endregion
    }
}