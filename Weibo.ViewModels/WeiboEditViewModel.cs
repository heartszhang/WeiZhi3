using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Weibo.Apis.SinaV2;

namespace Weibo.ViewModels
{
    public class WeiboEditViewModel : ViewModelBase2
    {
        private bool _is_busying;
        private string _reason;
        private string _body;
        private string _image;
        private readonly RelayCommand<IWeiboAccessToken> _submit;

        public bool is_busying { get { return _is_busying; } set { Set(ref _is_busying, value); } }
        public string reason { get { return _reason; } set { Set(ref _reason, value); } }
        public string body { get { return _body; } set { Set(ref _body, value); } }
        public string image { get { return _image; } set { Set(ref _image, value); } }
        public ICommand submit { get { return _submit; } }

        public WeiboEditViewModel()
        {
            _submit = new RelayCommand<IWeiboAccessToken>(ExecuteSubmit,CanExecuteSubmit);
        }

        private bool CanExecuteSubmit(IWeiboAccessToken arg)
        {
            return true;
        }

        private async void ExecuteSubmit(IWeiboAccessToken at)
        {
            is_busying = true;
            if (string.IsNullOrEmpty(image))
            {
                var resp = await WeiboClient.statuses_update_async(at.get(), _body);
                reason = resp.Reason;
            }else
            {
                if (!File.Exists(image))
                    reason = "图片文件不存在";
                var resp = await WeiboClient.statuses_upload_async(image, _body, at.get());
                reason = resp.Reason;
            }
            await Task.Delay(5000);
            is_busying = false;
        }
    }
}