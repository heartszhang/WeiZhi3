using Weibo.Apis.SinaV2;
using Weibo.DataModel;

namespace Weibo.ViewModels
{
    public class TimelineHomeViewModel : TimelineViewModel
    {
        public override void OnTick(string token)
        {

        }
        public override async void Reload(string token)
        {
            var ses = await WeiboClient.statuses_friends_timeline_refresh_async(token);
            if (ses.Failed())
            {
                FireNotificationMessage("{0} - timeline {1}", ses.Error(), ses.Reason);
                return;
            }
            FireNotificationMessage("{0} Status fetched", ses.Value.statuses.Length);

            ReloadSinaV2(ses.Value);

            MaxId = long.MinValue;
            MinId = long.MaxValue;
            SetMinMaxIds(ses.Value);

            PageNo = 1;
        }
        void SetMinMaxIds(Statuses sts)
        {
            foreach (var s in sts.statuses)
            {
                if (MaxId < s.id)
                    MaxId = s.id;
                if (MinId > s.id)
                    MinId = s.id;
            }
        }
        public override async void NextPage(string token)
        {
            var ses = await WeiboClient.statuses_friends_timeline_next_page_async(token,PageNo+1 );
            if(ses.Failed())
            {
                FireNotificationMessage("{0} - timeline {1}", ses.Error(), ses.Reason);
                return;
            }
            FireNotificationMessage("{1} - {0} Status fetched", ses.Value.statuses.Length, PageNo);
            ReloadSinaV2(ses.Value);
            ++PageNo;
            SetMinMaxIds(ses.Value);
        }

        public override async void PreviousPage(string token)
        {
            var ses = await WeiboClient.statuses_friends_timeline_next_page_async(token,PageNo -1);
            if (ses.Failed())
            {
                FireNotificationMessage("{0} - timeline {1}", ses.Error(), ses.Reason);
                return;
            }
            FireNotificationMessage("{1} - {0} Status fetched", ses.Value.statuses.Length, PageNo);
            ReloadSinaV2(ses.Value);
            ++PageNo;
            SetMinMaxIds(ses.Value);
        }
    }
}