using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Weibo.Api2.Sina;

namespace Weibo.ViewModels
{
    public abstract class TimelineViewModel:ViewModelBase2
    {
        public abstract void OnTick(string token);
        public abstract void Reload(string token);

        internal long previous_cursor { get; set; }
        internal long next_cursor { get; set; }
        internal long total_number { get; set; }
        
        internal void ReloadSina(dynamic result)
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
    public class PageHomeViewModel : TimelineViewModel
    {
       // private string _token;
        private UserExt _user;
        public long uid { get; set; }

        public UserExt user
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }
        public PageHomeViewModel(long id, string at)
        {
            uid = id;
            //_token = at;
            //Initialize(at);
        }
        public async void Initialize(string at)
        {
            if (IsInDesignMode)
                return;
             var r = await SinaClient.users_show(at,uid);
             if (!r.Failed())
             {
                 var u = new UserExt();
                 u.assign_sina(r.Result);
                 user = u;
                 Reload(at);
             }
             else FireFailedMessage(r.Error(),r.Reason(),"user_show");
        }

        public override void OnTick(string token)
        {
            
        }

        public override async void Reload(string token)
        {
            var wr = await SinaClient.home_timeline(token);
            if(wr.Failed())
            {
                FireFailedMessage(wr.Error(), wr.Reason(), "timeline");
                return;
            }
            ReloadSina(wr.Result);
        }
        void FireFailedMessage(int error, string reason, string source)
        {
            Messenger.Default.Send(new NotificationMessage(string.Format("{0}:{1}", error, reason)), source);
        }
    }
}