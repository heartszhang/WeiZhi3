using System.Threading.Tasks;

namespace Weibo.ViewModels
{
    public abstract class NavigationItem : ObservableObjectExt
    {
        protected string _text;
        protected string _image;
        protected int _total_number;
        protected int total_number { get { return _total_number; }
            set { if (_total_number != 0) notifications = value - _total_number;
                _total_number = value;
            }
        }
        protected int _notifications;

        public string text
        {
            get { return _text; }
            set { Set(ref  _text , value); }
        }

        public string image
        {
            get { return _image; }
            set { Set(ref _image , value); }
        }

        protected NavigationItem(string t)
        {
            tag = t;
        }
        public string tag { get; set; }
        public int notifications
        {
            get { return _notifications; }
            set { Set(ref _notifications, value); }
        }

        protected int _ticks;
        protected long _since_id = 0;
        public abstract void OnTick(IWeiboAccessToken at) ;
    }
}