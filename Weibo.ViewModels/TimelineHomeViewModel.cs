using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Caching;
using GalaSoft.MvvmLight.Messaging;
using Weibo.Apis;
using Weibo.Apis.SinaV2;
using Weibo.DataModel;

namespace Weibo.ViewModels
{
    public class TimelineHomeViewModel : TimelineViewModel
    {
        public override void OnTick(IWeiboAccessToken token)
        {

        }
        public override async void Reload(IWeiboAccessToken token)
        {
            var ses = await WeiboClient.statuses_friends_timeline_refresh_async(token.get());
            if (ses.Failed())
            {
                FireNotificationMessage("{0} - timeline {1}", ses.Error(), ses.Reason);
                return;
            }
            FireNotificationMessage("{0} Status fetched", ses.Value.statuses.Length);
            Messenger.Default.Send(new WeiboMediaReset());
            await FetchUrlInfos(ses.Value.statuses);

            ReloadSinaV2(ses.Value,true);

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
        public override void NextPage(IWeiboAccessToken token)
        {
            SecondPage(token.get(),true);
            ++PageNo;
        }
        async void SecondPage(string token, bool reload)
        {
            if (next_cursor == 0)
            {
                FireNotificationMessage("nothing should be fetched", 0);
                return;
            }

            var ses = await WeiboClient.statuses_friends_timeline_next_page_async(token, 1, MinId);
            if (ses.Failed())
            {
                FireNotificationMessage("{0} - timeline {1}", ses.Error(), ses.Reason);
                return;
            }
            FireNotificationMessage("{1} - {0} Status fetched", ses.Value.statuses.Length, PageNo);
            //Messenger.Default.Send(new WeiboMediaReset());

            await FetchUrlInfos(ses.Value.statuses);

            ReloadSinaV2(ses.Value, reload);
            SetMinMaxIds(ses.Value);
        }
        public override void MorePage(IWeiboAccessToken token)
        {
            SecondPage(token.get(), false);
        }
    }
}