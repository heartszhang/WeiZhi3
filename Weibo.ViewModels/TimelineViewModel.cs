using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using Weibo.Apis;
using Weibo.Apis.SinaV2;
using Weibo.DataModel;

namespace Weibo.ViewModels
{
    public abstract class TimelineViewModel:ViewModelBase2
    {
        public abstract void OnTick(string token);
        public abstract void Reload(string token);

        public abstract void NextPage(string token);
        public abstract void MorePage(string token);

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

        internal void ReloadSinaV2(Statuses result,bool reload )
        {
            previous_cursor = result.previous_cursor;
            next_cursor = result.next_cursor;
            total_number = result.total_number;

            if(reload)
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
            DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.SystemIdle,act);
        }
        public ObservableCollection<WeiboStatus> statuses { get; set; }
        protected TimelineViewModel()
        {
            statuses = new ObservableCollection<WeiboStatus>();
        }
        protected async Task FetchUrlInfos(Status[] ses)
        {
            if (ses == null || ses.Length == 0)
                return;
            var urls = new HashSet<string>();
            foreach (var s in ses)
            {
                var us = Utils.ExtractUrlFromWeibo(s.text);
                foreach(var url in us)
                {
                    urls.Add(url);
                }
                //urls.Add(us);
                if (s.retweeted_status != null)
                {
                    var rus = Utils.ExtractUrlFromWeibo(s.retweeted_status.text);
                    foreach (var url in rus)
                    {
                        urls.Add(url);
                    }
                }
            }
            if(urls.Count >= 20)
            {
                var tasks = new Task[2];
                var u1 = urls.Take(20);
                tasks[0] = FetchUrlInfosImp(u1);
                var u2 = urls.Skip(20);
                tasks[1] = FetchUrlInfosImp(u2);
                await Task.WhenAll(tasks);
            }else
            {
                await FetchUrlInfosImp(urls);
            }

        }
        async Task FetchUrlInfosImp(IEnumerable<string> urls)
        {
            var rlt = await WeiboClient.short_url_info(urls, 0);
            if (rlt.Failed())
                return;
            var mem = MemoryCache.Default;
            foreach (var url in rlt.Value.urls)
            {
                mem.Set(url.url_short, url, DateTimeOffset.Now.AddHours(2.0));
                Debug.WriteLine("url-cache {3} - {0} : {1} - {2}, {4}", url.type, url.topic(), url.url_long, url.url_short
                    ,url.description);
            }
        }
    }
}