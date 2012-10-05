using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Weibo.Apis.SinaV2;

namespace Weibo.ViewModels
{
    public class CommentReply : ObservableObjectExt
    {
        private readonly long _status_id;
        private readonly long _comment_id;
        private bool _is_busying;
        private bool _without_mention;
        private bool _comment_ori;
        private string _body;
        private string _reason_phrase;
        private readonly RelayCommand<IWeiboAccessToken> _reply;

        public bool is_busying { get { return _is_busying; } set { Set(ref _is_busying, value); } }
        public bool without_mention { get { return _without_mention; } set { Set(ref _without_mention, value); } }
        public bool comment_ori { get { return _comment_ori; } set { Set(ref _comment_ori, value); } }
        public ICommand reply { get { return _reply; } }
        public string body { get { return _body; } set { Set(ref _body, value); } }
        public string reason_phrase { get { return _reason_phrase; } set { Set(ref _reason_phrase, value); } }

        public CommentReply(long sid,long commentid)
        {
            _comment_id = commentid;
            _status_id = sid;
            _reply = new RelayCommand<IWeiboAccessToken>(ExecuteReply, CanExecuteReply);
        }

        private bool CanExecuteReply(IWeiboAccessToken arg)
        {
            return true;
        }

        private async void ExecuteReply(IWeiboAccessToken token)
        {
            is_busying = true;
            var resp =
                await
                WeiboClient.comments_reply_async(_comment_id, _status_id, _body, _without_mention, _comment_ori,
                                                 token.get());
            DispatcherHelper.CheckBeginInvokeOnUI(() => reason_phrase = resp.Reason);
            await Task.Delay(5000);
            DispatcherHelper.CheckBeginInvokeOnUI(() => is_busying = false);
        }
    }
}