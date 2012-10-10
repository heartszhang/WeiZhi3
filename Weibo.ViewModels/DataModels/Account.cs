using System.Runtime.Serialization;
using Weibo.DataModel;

namespace Weibo.ViewModels.DataModels
{
    [DataContract]
    public class Account : IWeiboAccessToken
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string AccessToken { get; set; }

        [DataMember]
        public long Expired { get; set; }

        [DataMember]
        public WeiboSourcesType Source { get; set; }

        [DataMember]
        public int TimelineTotalNumber { get; set; }

        [DataMember]
        public int CommentsTimelineTotalNumber { get; set; }

        #region Implementation of IWeiboAccessToken

        string IWeiboAccessToken.get()
        {
            return AccessToken;
        }

        long IWeiboAccessToken.id()
        {
            return Id;
        }

        int IWeiboAccessToken.count_per_page()
        {
            return 50;
        }

        int IWeiboAccessToken.comments_timeline_total_number()
        {
            return 0;
        }

        int IWeiboAccessToken.timeline_total_number()
        {
            return 0;
        }

        #endregion
    }
}