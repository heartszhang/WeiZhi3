using System;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

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
        protected static void UiInvoke(Action act)
        {
            DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.SystemIdle, act);
        }

        protected long MinId = long.MaxValue;
        protected long MaxId = long.MinValue;

        protected long previous_cursor { get; set; }
        protected long next_cursor { get; set; }
        protected int total_number 
        {
            get { return _total_number; } 
            set
            {
                if(_total_number != 0)
                    notifications= value - _total_number;
                _total_number = value;
            } 
        }

        protected int _total_number;
        protected int _notifications;
        public int notifications { get { return _notifications; } set { Set(ref _notifications, value); } }
    }
}