using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Weibo.DataModel;

namespace Weibo.ViewModels
{
    public abstract  class CommentlineViewModel :ViewModelBase2
    {
        private readonly RelayCommand<IWeiboAccessToken> _reload;
        private readonly RelayCommand<IWeiboAccessToken> _next_page;

        public ObservableCollection<WeiboComment> statuses { get; set; }//as comments

        public ICommand ReloadCommand { get { return _reload; } }
        public ICommand NextPageCommand { get { return _next_page; } }

        public abstract void Reload(string token);

        public abstract void NextPage(string token);
        public abstract void MorePage(string token);

        protected CommentlineViewModel()
        {
            _reload = new RelayCommand<IWeiboAccessToken>(ExecuteReload);
            _next_page = new RelayCommand<IWeiboAccessToken>(ExecuteNextPage);

            statuses = new ObservableCollection<WeiboComment>();
        }

        private void ExecuteNextPage(IWeiboAccessToken obj)
        {
            NextPage(obj.get());
        }

        private void ExecuteReload(IWeiboAccessToken obj)
        {
            Reload(obj.get());
        }

        protected void ReloadSinaV2(Comments comments ,bool reload)
        {
            if (reload)
                UiInvoke(() => statuses.Clear());
            foreach (var s in comments.comments)
            {
                var ws = new WeiboComment();
                ws.assign_sina(s);
                UiInvoke(() => statuses.Add(ws));
            }            
        }
    }
}