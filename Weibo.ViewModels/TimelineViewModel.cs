using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using Weibo.Apis.SinaV2;
using Weibo.DataModel;

namespace Weibo.ViewModels
{
    public abstract class TimelineViewModel:ViewModelBase2
    {
        public abstract void OnTick(string token);
        public abstract void Reload(string token);

        internal long previous_cursor { get; set; }
        internal long next_cursor { get; set; }
        internal long total_number { get; set; }
        
       /* internal void ReloadSina(dynamic result)
        {
            previous_cursor = result.previous_cursor;
            next_cursor = result.next_cursor;
            total_number = result.total_number;

            UiInvoke(()=>statuses.Clear());
            foreach(var s in result.statuses)
            {
                var ws = new WeiboStatus();
                ws.assign_sina(s);
                UiInvoke(()=>statuses.Add(ws));
            }
        }*/
        
        internal void ReloadSinaV2(Statuses result)
        {
            previous_cursor = result.previous_cursor;
            next_cursor = result.next_cursor;
            total_number = result.total_number;

            UiInvoke(() => statuses.Clear());
            foreach (var s in result.statuses)
            {
                var ws = new WeiboStatus();
                ws.assign_sina(s);
                UiInvoke(() => statuses.Add(ws));
            }
        }
        static void UiInvoke( Action act)
        {
            DispatcherHelper.UIDispatcher.Invoke(DispatcherPriority.SystemIdle,act);
        }
        public ObservableCollection<WeiboStatus> statuses { get; set; }
        protected TimelineViewModel()
        {
            statuses = new ObservableCollection<WeiboStatus>();
        }
    }
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
            else
            {
                FireNotificationMessage("{0} Status fetched", ses.Value.statuses.Length);
            }
            ReloadSinaV2(ses.Value);
        }
    }
}