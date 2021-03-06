using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class UserWithoutStatus
    {
        [DataMember]
        public string profile_image_url { get; set; }
        [DataMember]
        public string screen_name { get; set; }
        [DataMember]
        public bool verified { get; set; }
        [DataMember]
        public long id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int province { get; set; }
        [DataMember]
        public int city { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string domain { get; set; }
        [DataMember]
        public string gender { get; set; }//m/f
        [DataMember]
        public int followers_count { get; set; }
        [DataMember]
        public int friends_count { get; set; }
        [DataMember]
        public int statuses_count { get; set; }
        [DataMember]
        public int favourites_count { get; set; }
        [DataMember]
        public string created_at { get; set; }
        [DataMember]
        public bool following { get; set; }
        [DataMember]
        public bool allow_all_act_msg { get; set; }
        [DataMember]
        public bool geo_enabled { get; set; }

        [DataMember]
        public int bi_followers_count { get; set; }

        [DataMember]
        public int online_status { get; set; }

        [DataMember]
        public bool follow_me { get; set; }

        [DataMember]
        public string verified_reason { get; set; }

        [DataMember]
        public int verified_type { get; set; }

        [DataMember]
        public string avatar_large { get; set; }
        [DataMember]
        public bool allow_all_comment { get; set; }

        [DataMember]
        public string weihao { get; set; }

        //        [DataMember]
        //        public string profile_url { get; set; }
        [DataMember]
        public string lang { get; set; }
    }
}