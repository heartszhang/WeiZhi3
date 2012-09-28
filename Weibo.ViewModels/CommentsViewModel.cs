using System.Collections.ObjectModel;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using Weibo.Apis.SinaV2;
using Weibo.DataModel;
 
namespace Weibo.ViewModels
{
    public class CommentsViewModel : ViewModelBase2
    {
#region private members
        private long _previous_cursor;
        private long _next_cursor;
        private int _total_number;
        private string _reason = "\u2205; 正在获取微博...";
#endregion

        internal long id { get; set; }
        public ObservableCollection<WeiboComment> comments { get; set; }
        public string reason { get { return _reason; } set { Set(ref _reason, value); } }

        public CommentsViewModel(long sid)
        {
            id = sid;
            comments = new ObservableCollection<WeiboComment>();
        }
        public async void initialize(string token)
        {
            var resp = await WeiboClient.comments_show_refresh_async(id, 0, 20, 1, token);
            var rsn = string.Format("{0} comments {1}", resp.Value.total_number, resp.Reason);
            if(resp.Failed())
            {
                //UiBeginInvoke();
                UiInvoke(()=>reason = rsn);
                FireNotificationMessage(rsn);
                return;
            }
            UiInvoke(()=>reason = rsn);
            FireNotificationMessage(rsn);
            assign_comments(resp.Value);
        }
        void UiBeginInvoke(System.Action act)
        {
            DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.SystemIdle, act);
        }
        void UiInvoke(System.Action act)
        {
            DispatcherHelper.UIDispatcher.Invoke(DispatcherPriority.SystemIdle, act);
        }
        void assign_comments(Comments cmts)
        {
            _total_number = cmts.total_number;
            _next_cursor = cmts.next_cursor;
            _previous_cursor = cmts.previous_cursor;
            foreach(var cmt in cmts.comments)
            {
                var c = new WeiboComment();
                c.assign_sina(cmt);
                UiInvoke(()=> comments.Add(c));
            }
        }
    }
}