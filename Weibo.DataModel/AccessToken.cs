using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class AccessToken
    {
        [DataMember]
        public string access_token { get; set; }

        [DataMember]
        public int expires_in { get; set; }

        [DataMember]
        public string error { get; set; }

        [DataMember]
        public int error_code { get; set; }

        [DataMember]
        public string error_description { get; set; }

		[DataMember]
		public long uid{get;set;}
    }
}