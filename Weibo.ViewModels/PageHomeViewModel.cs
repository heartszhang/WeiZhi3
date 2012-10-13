using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Weibo.Apis.SinaV2;

namespace Weibo.ViewModels
{
    public class PageHomeViewModel : ViewModelBase2
    {
        private readonly Recomendations _recomendations;
        private ViewModelBase2 _content;
        private CommentlineViewModel _comments;
        private UserExt _user;
        private TimelineViewModel _timeline;
        public long uid { get; set; }

        private object _second_content;
        public object SecondContent { get { return _second_content; } set { Set(ref _second_content, value); } }

       // public NavigationItem CommentsToMeItem { get; set; }
        //public NavigationItem CommentsByMeItem { get; set; }
        public NavigationItem CommentsTimelineItem { get; set; }
        public NavigationItem HomeTimelineItem { get; set; }
        public Recomendations Recomendations { get { return _recomendations; } }

        private readonly RelayCommand<IWeiboAccessToken> _show_comments_to_me;
        public ICommand ShowCommentsToMe { get { return _show_comments_to_me; } }
        private readonly RelayCommand<IWeiboAccessToken> _show_home_timeline;
        public ICommand ShowHomeTimeline { get { return _show_home_timeline; } }
        private readonly RelayCommand<IWeiboAccessToken> _show_comments_by_me;
        public ICommand ShowCommentsByMe { get { return _show_comments_by_me; } }
        private readonly RelayCommand<IWeiboAccessToken> _show_comments_timeline;
        public ICommand ShowCommentsTimeline { get { return _show_comments_timeline; } }

        //ShowCommentsByMe
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
        public CommentlineViewModel Commentline
        {
            get { return _comments; }
            set { Set(ref _comments, value); }
        }
        public ViewModelBase2 Content { get { return _content; } set { Set(ref _content, value); } }

        public PageHomeViewModel(long id, string at)
        {
            uid = id;
            Media = new MediaCollectionViewModel();
            _show_comments_to_me =new RelayCommand<IWeiboAccessToken>(ExecuteShowCommentsToMe);
            _show_comments_by_me = new RelayCommand<IWeiboAccessToken>(ExecuteShowCommentsByMe);
            _show_comments_timeline = new RelayCommand<IWeiboAccessToken>(ExecuteShowCommentsTimeline);
            _show_home_timeline = new RelayCommand<IWeiboAccessToken>(ExecuteShowHomeTimeline);
//            CommentsToMeItem = new NavigationItem("M");
//            CommentsByMeItem = new NavigationItem("X");
            CommentsTimelineItem = new CommentsTimelineNavigationItem();
            HomeTimelineItem = new HomeTimelineNavigationItem();
            _recomendations = new Recomendations();
        }

        private void ExecuteShowHomeTimeline(IWeiboAccessToken obj)
        {
            Content = Timeline;
        }

        private void ExecuteShowCommentsTimeline(IWeiboAccessToken at)
        {
            var mv = new CommentTimelineViewModel();
            mv.Reload(at.get());
            Commentline = mv;
            Content = mv;
        }
        private void ExecuteShowCommentsByMe(IWeiboAccessToken at)
        {
            var mv = new CommentlineByMeViewModel();
            mv.Reload(at.get());
            Commentline = mv;
            Content = mv;
        }

        private void ExecuteShowCommentsToMe(IWeiboAccessToken at)
        {
            var mv = new CommentlineToMeViewModel();
            mv.Reload(at.get());
            Commentline = mv;
            Content = mv;
        }

        public async void Initialize(IWeiboAccessToken at)
        {
            if (IsInDesignMode)
                return;
            var t = WeiboClient.users_show_async(uid, at.get());

            Timeline = new TimelineHomeViewModel();
            Content = Timeline;

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
 //           await Task.Delay(2000);
            
        }

        public void OnTick(IWeiboAccessToken token)
        {
            SwitchSecondContent();
            if(Timeline != null)
                Timeline.OnTick(token);
            CommentsTimelineItem.OnTick(token);
            HomeTimelineItem.OnTick(token);
            Recomendations.OnTick(token);
        }
        public override void Cleanup()
        {
            base.Cleanup();
            Media.Cleanup();
            Timeline.Cleanup();
        }

        private int _switch_second_tick;
        void SwitchSecondContent()
        {
            if (_switch_second_tick++%47 != 5)
                return;
            if (Recomendations.statuses.Count == 0)
                return;
            var ws = Recomendations.statuses.LastOrDefault();
            Recomendations.statuses.Remove(ws);
            SecondContent = ws;
        }
    }
}