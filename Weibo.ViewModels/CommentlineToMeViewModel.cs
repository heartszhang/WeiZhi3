using Weibo.Apis.SinaV2;
using Weibo.DataModel;

namespace Weibo.ViewModels
{
    public class CommentlineToMeViewModel : CommentlineViewModel
    {
        #region Overrides of CommentlineViewModel

        public override async void Reload(string token)
        {
            var ses = await WeiboClient.comments_to_me_refresh_async(token, 0, 50, 1);
            if (ses.Failed())
            {
                FireNotificationMessage("{0} - comments {1}", ses.Error(), ses.Reason);
                return;
            }
            FireNotificationMessage("{0} comments fetched", ses.Value.comments.Length);
            ReloadSinaV2(ses.Value, true);

            MaxId = long.MinValue;
            MinId = long.MaxValue;
            SetMinMaxIds(ses.Value);

            //  PageNo = 1;
        }
        void SetMinMaxIds(Comments comments)
        {
            foreach(var cmt in comments.comments)
            {
                if (MaxId < cmt.id)
                    MaxId = cmt.id;
                if (MinId > cmt.id)
                    MinId = cmt.id;
            }
        }
        public override void NextPage(string token)
        {
            SecondPage(token, true);
        }
        public async void SecondPage(string token,bool reload)
        {
            var ses = await WeiboClient.comments_to_me_next_page_async(token , MinId-1, 50, 1);

            if (ses.Failed())
            {
                FireNotificationMessage("{0} - comments {1}", ses.Error(), ses.Reason);
                return;
            }
            FireNotificationMessage("{0} comments fetched", ses.Value.comments.Length);
            ReloadSinaV2(ses.Value, reload);
            SetMinMaxIds(ses.Value);
        }

        public override void MorePage(string token)
        {
            SecondPage(token, false);
        }

        #endregion
    }
}