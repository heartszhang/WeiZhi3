using GalaSoft.MvvmLight.Messaging;
using Weibo.Api2.Sina;

namespace Weibo.ViewModels
{
    public class PageHomeViewModel : ViewModelBase2
    {
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
            if (IsInDesignMode)
                return;
            Initialize(at);
        }
        async void Initialize(string at)
        {
             var r = await SinaClient.users_show(at,uid);
             if (!r.Failed())
             {
                 var u = new UserExt();
                 u.assign_sina(r.Result);
                 user = u;
             }
             else Messenger.Default.Send(new NotificationMessage(string.Format("{0}:{1}",r.Error(), r.Reason())),"user_show");
        }
    }
}