using Weibo.Apis.SinaV2;

namespace Weibo.ViewModels
{
    public class PageHomeViewModel : ViewModelBase2
    {
        // private string _token;
        private UserExt _user;
        private TimelineViewModel _timeline;
        public long uid { get; set; }

        public MediaCollectionViewModel Media { get; set; }
        public UserExt user
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }

        public TimelineViewModel Timeline
        {
            get { return _timeline; }
            set { Set(ref _timeline , value); }
        }

        public PageHomeViewModel(long id, string at)
        {
            uid = id;
            Media = new MediaCollectionViewModel();
        }
        public async void Initialize(string at)
        {
            if (IsInDesignMode)
                return;
            var t = WeiboClient.users_show_async(uid, at);

            Timeline = new TimelineHomeViewModel();
            Timeline.Reload(at);
            var r = await t;
            if (!r.Failed())
            {
                var u = new UserExt();
                u.assign_sina(r.Value);
                user = u;
                FireNotificationMessage("@{0} Wecolme",u.screen_name);   
            }
            else FireNotificationMessage("{0} - user {1}",r.Error(), r.Reason);
        }

        public void OnTick(string token)
        {
            if(Timeline != null)
                Timeline.OnTick(token);
        }
        public override void Cleanup()
        {
            base.Cleanup();
            Media.Cleanup();
            Timeline.Cleanup();
        }
    }
}