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
        public abstract void OnTick(IWeiboAccessToken token);
        public abstract void Reload(IWeiboAccessToken token);

        public abstract void NextPage(IWeiboAccessToken token);
        public abstract void MorePage(IWeiboAccessToken token);

        private object _focused;
        protected int PageNo = 1;

        public object FocusedItem
        {
            get { return _focused; }
            set { Set(ref _focused, value); }
        }

        internal void ReloadSinaV2(Statuses result,bool reload )
        {
            previous_cursor = result.previous_cursor;
            next_cursor = result.next_cursor;
            total_number = result.total_number;
            Debug.WriteLine("noti: {3}, timeline total: {0},prev: {1},next: {2}",total_number,previous_cursor, next_cursor, notifications);

            if(reload)
                UiInvoke(() => statuses.Clear());
            foreach (var s in result.statuses)
            {
                var ws = new WeiboStatus();
                ws.assign_sina(s);
                UiInvoke(() => statuses.Add(ws));
            }
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
            var mem = MemoryCache.Default;

            var urls = new HashSet<string>();
            foreach (var s in ses)
            {
                var us = Utils.ExtractUrlFromWeibo(s.text);
                foreach(var url in us)
                {
                    if (mem.Get("http://t.cn/" + url) == null)
                        urls.Add(url);
                }
                //urls.Add(us);
                if (s.retweeted_status != null)
                {
                    var rus = Utils.ExtractUrlFromWeibo(s.retweeted_status.text);
                    foreach (var url in rus)
                    {
                        if (mem.Get("http://t.cn/" + url) == null)
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