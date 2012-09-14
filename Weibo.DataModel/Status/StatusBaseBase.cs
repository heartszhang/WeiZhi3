using System.Runtime.Serialization;

namespace Weibo.DataModel.Status
{
    [DataContract]
    public class StatusBaseBase
    {
        [DataMember]
        public string thumbnail_pic { get; set; }
        [DataMember]
        public string bmiddle_pic { get; set; }
        [DataMember]
        public string original_pic { get; set; }
        [DataMember]
        public string text { get; set; }
        [DataMember]
        public string source { get; set; }
        [DataMember]
        public long id { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public bool favorited { get; set; }
        [DataMember]
        public bool truncated { get; set; }
        [DataMember]
        public string in_reply_to_status_id { get; set; }
        [DataMember]
        public string in_reply_to_user_id { get; set; }
        [DataMember]
        public string in_reply_to_screen_name { get; set; }
        //        [DataMember]
        //        public StatusMeta[] annotations { get; set; }
        [DataMember]
        public int reposts_count { get; set; }
        [DataMember]
        public int comments_count { get; set; }
        [DataMember]
        public long mid { get; set; }
        [DataMember]
        public string idstr { get; set; }

        [DataMember]
        public int mlevel { get; set; }

        [DataMember]
        public StatusGeo geo { get; set; }

        [DataMember]
        public StatusVisible visible { get; set; }

        //added by weizhi
        [DataMember]
        public int thumb_pic_width { get; set; }
        [DataMember]
        public int thumb_pic_height { get; set; }
        [DataMember]
        public string url_topic { get; set; }
        [DataMember]
        public string url_short { get; set; }
        [DataMember]
        public int url_type { get; set; }
        [DataMember]
        public string url_long { get; set; }

        [DataMember]
        public string widget { get; set; }

        [DataMember]
        public int mark { get; set; }
        [DataMember]
        public string html_file_path { get; set; }

        [DataMember]
        public bool is_touched { get; set; }
    }
}