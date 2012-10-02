using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

namespace Weibo.ViewModels
{
    internal class WeiboMediaReset
    {

    }

    public class MediaCollectionViewModel : ViewModelBase2
    {
        private readonly ObservableCollection<WeiboUrl> _medias = new ObservableCollection<WeiboUrl>();
        public ObservableCollection<WeiboUrl> Medias { get { return _medias; } }
        public MediaCollectionViewModel()
        {
            Messenger.Default.Register<WeiboUrl>(this,OnFetchWeiboUrl);
            Messenger.Default.Register<WeiboMediaReset>(this, OnReset);
        }

        private void OnReset(WeiboMediaReset obj)
        {
            DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.SystemIdle, (Action)(() => Medias.Clear()));
        }

        private void OnFetchWeiboUrl(WeiboUrl obj)
        {
            DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.SystemIdle, (Action)(() =>
            {                
                if(!Medias.Any(item=>item.data.url_short.Equals(obj.data.url_short)))
                    Medias.Add(obj);
            }));
        }
    }
}