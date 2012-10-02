using System.Collections.ObjectModel;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Weibo.Apis.SinaV2;
using Weibo.DataModel;
using System.Windows.Input;
using System.Diagnostics;
 
namespace Weibo.ViewModels
{
    public class CommentsViewModel : ViewModelBase2
    {
#region private members
        private long _previous_cursor;
        private long _next_cursor;
        private int _total_number = int.MaxValue;
        private int _page = 1;
        private const int item_per_page = 20;
        private string _reason = "\u2205; 正在获取微博...";
#endregion

        internal long id { get; set; }
        public ObservableCollection<WeiboComment> comments { get; set; }
        public string reason { get { return _reason; } set { Set(ref _reason, value); } }
        public ICommand show_more { get; set; }

        public CommentsViewModel(long sid)
        {
            id = sid;
            comments = new ObservableCollection<WeiboComment>();
            show_more = new RelayCommand<IWeiboAccessToken>(execute_show_more,can_show_more);
        }

        private bool can_show_more(IWeiboAccessToken at)
        {// IsEnabled="{Binding can_show_more,Mode=OneWay}"
            return (_page - 1) * item_per_page < _total_number;
        }
        public async void initialize(string token)
        {
            var resp = await WeiboClient.comments_show_refresh_async(id, 0, item_per_page, _page, token);
            var rsn = string.Format("{0} comments {1}", resp.Value.total_number, resp.Reason);
            if(resp.Failed())
            {
                UiBeginInvoke(()=>reason = rsn);
                FireNotificationMessage(rsn);
                return;
            }
            ++_page;
            UiBeginInvoke(()=>reason = rsn);
            FireNotificationMessage(rsn);
            assign_comments(resp.Value);
            UiBeginInvoke(() => ((RelayCommand<IWeiboAccessToken>)show_more).RaiseCanExecuteChanged());
        }
        void execute_show_more(IWeiboAccessToken at)
        {
            Debug.Assert(at != null);
            initialize(at.get());
        }
        void UiBeginInvoke(System.Action act)
        {
            DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.SystemIdle, act);
        }
        /*void UiInvoke(System.Action act)
        {
            DispatcherHelper.UIDispatcher.Invoke(DispatcherPriority.SystemIdle, act);
        }*/
        void assign_comments(Comments cmts)
        {
            _total_number = cmts.total_number;
            _next_cursor = cmts.next_cursor;
            _previous_cursor = cmts.previous_cursor;
            //cmts.comments.Sort((l, r) => { return -1; });
            foreach(var c in cmts.comments)
            {
                c.preprocess();
            }
            System.Array.Sort(cmts.comments, (l, r) => r.score.CompareTo(l.score));
            foreach(var cmt in cmts.comments)
            {
                if (cmt.score < 4)
                    continue;
                var c = new WeiboComment();
                c.assign_sina(cmt);
                UiBeginInvoke(()=> comments.Add(c));
            }
        }
    }
}