using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Weibo.Apis.SinaV2;

namespace Weibo.ViewModels
{
    public class WeiboReply:ObservableObjectExt
    {
        private readonly long _status_id;
        private bool _is_busying;
        private bool _reply_to_original;
        private string _body;
        private string _reason_phrase;
        private readonly RelayCommand<IWeiboAccessToken> _reply;
        private readonly RelayCommand<IWeiboAccessToken> _retweet;

        public bool is_busying { get { return _is_busying; } set { Set(ref _is_busying, value); } }
        public bool reply_to_original { get { return _reply_to_original; } set { Set(ref _reply_to_original, value); } }
        public ICommand reply { get { return _reply; }  }
        public ICommand retweet { get { return _retweet; }  }
        public string body { get { return _body; } set { Set(ref _body, value); } }
        public string reason_phrase { get { return _reason_phrase; } set { Set(ref _reason_phrase, value); } }
        public string mention { get; set; }
        public string refer { get; set; }

        public WeiboReply(long sid, string text, string user)
        {
            _status_id = sid;
            _reply = new RelayCommand<IWeiboAccessToken>(ExecuteReply, CanExecuteReply);
            _retweet = new RelayCommand<IWeiboAccessToken>(ExecuteRetweet, CanExecuteRetweet);
            mention = user;
            refer = user + "£º" + text;
        }

        private bool CanExecuteRetweet(IWeiboAccessToken arg)
        {
            return true;
        }

        private async void ExecuteRetweet(IWeiboAccessToken token)
        {
            is_busying = true;
            var r = reply_to_original
                        ? WeiboClient.StatusesRepostIsCommentFlag.CommentOrignal
                        : WeiboClient.StatusesRepostIsCommentFlag.NoComment;
            var resp = await WeiboClient.statuses_repost_async(body, _status_id, r, token.get());
            DispatcherHelper.CheckBeginInvokeOnUI(()=>reason_phrase = resp.Reason);
            await Task.Delay(5000);
            DispatcherHelper.CheckBeginInvokeOnUI(()=>is_busying = false);
        }

        private bool CanExecuteReply(IWeiboAccessToken arg)
        {
            return true;
        }

        private async void ExecuteReply(IWeiboAccessToken token)
        {
            is_busying = true;
            var resp = await WeiboClient.comments_create_async(body, _status_id, reply_to_original,token.get());
            DispatcherHelper.CheckBeginInvokeOnUI(()=>reason_phrase = resp.Reason);
            await Task.Delay(5000);
            DispatcherHelper.CheckBeginInvokeOnUI(()=>is_busying = false);
        }
    }
}