using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using Weibo.DataModel;

namespace Weibo.ViewModels
{
    public abstract class TimelineViewModel:ViewModelBase2
    {
        public abstract void OnTick(string token);
        public abstract void Reload(string token);

        public abstract void NextPage(string token);
        public abstract void PreviousPage(string token);

        internal long previous_cursor { get; set; }
        internal long next_cursor { get; set; }
        internal long total_number { get; set; }

        private object _focusedItem;
        protected long MinId = long.MaxValue;
        protected long MaxId = long.MinValue;
        protected int PageNo = 1;

        public object FocusedItem
        {
            get { return _focusedItem; }
            set { Set(ref _focusedItem, value); }
        }

        internal void ReloadSinaV2(Statuses result)
        {
            previous_cursor = result.previous_cursor;
            next_cursor = result.next_cursor;
            total_number = result.total_number;

            UiInvoke(() => statuses.Clear());
            foreach (var s in result.statuses)
            {
                var ws = new WeiboStatus();
                ws.assign_sina(s);
                UiInvoke(() => statuses.Add(ws));
            }
        }
        static void UiInvoke( Action act)
        {
            DispatcherHelper.UIDispatcher.Invoke(DispatcherPriority.SystemIdle,act);
        }
        public ObservableCollection<WeiboStatus> statuses { get; set; }
        protected TimelineViewModel()
        {
            statuses = new ObservableCollection<WeiboStatus>();
        }
    }
}