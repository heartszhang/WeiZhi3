using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace Weibo.ViewModels
{
    public abstract class ViewModelBase2 : ViewModelBase
    {
        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            Set(propertyName, ref storage, value);
            return true;
        }
        protected void FireNotificationMessage(string format, params object[] args)
        {
            Messenger.Default.Send(new NotificationMessage(string.Format(format, args)), "noti");
        }
    }
}