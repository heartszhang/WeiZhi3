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
//            UiInvoke(()=>    CollectionViewSource.GetDefaultView(statuses).MoveCurrentTo(statuses[0]));
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
        protected void FetchUrlInfos(Status[] ses)
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
                var u1 = urls.Take(20);
                Task.Run(()=> FetchUrlInfosImp(u1));
                var u2 = urls.Skip(20);
                Task.Run(()=> FetchUrlInfosImp(u2));
            }else
            {
                FetchUrlInfosImp(urls);
            }

        }
        async void FetchUrlInfosImp(IEnumerable<string> urls)
        {
            var rlt = await WeiboClient.short_url_info(urls, 0);
            if (rlt.Failed())
                return;
            var mem = MemoryCache.Default;
            foreach (var url in rlt.Value.urls)
            {
                mem.Set(url.url_short, url, DateTimeOffset.Now.AddHours(2.0));
                url.title = ExtractUrlTitle(url.title);
                Debug.WriteLine("url-cache {3} - {0} : {1} - {2}", url.type, url.title, url.url_long, url.url_short);
            }
        }
        static readonly Regex ArticleTitleDashRegex1 = new Regex(" [\\|\\-_] ", RegexOptions.Compiled);
        static readonly Regex ArticleTitleDashRegex2 = new Regex("(.*?)[\\|\\-_].*", RegexOptions.Compiled);
        static readonly Regex ArticleTitleDashRegex3 = new Regex("[^\\|\\-_]*[\\|\\-_](.*)", RegexOptions.Compiled);
        static readonly Regex ArticleTitleColonRegex1 = new Regex(".*[:：](.*)", RegexOptions.Compiled);
        static readonly Regex ArticleTitleColonRegex2 = new Regex("[^:]*[:](.*)", RegexOptions.Compiled);    
        string ExtractUrlTitle(string title)
        {
            var rtn = title;
            if(string.IsNullOrEmpty(title))
                return rtn;
            if (ArticleTitleDashRegex1.IsMatch(title))
            {
                rtn = ArticleTitleDashRegex2.Replace(title, "$1");

                //中文不能用空格计算字数
                if (title.Length < 5)
                {
                    rtn = ArticleTitleDashRegex3.Replace(title, "$1");
                }
            }
            else if ((title.IndexOf(": ") != -1) || (title.IndexOf("：") != -1))
            {
                rtn = ArticleTitleColonRegex1.Replace(title, "$1");

                if (title.Length < 3)
                {
                    rtn = ArticleTitleColonRegex2.Replace(title, "$1");
                }
            }
            return rtn;
        }
    }
}