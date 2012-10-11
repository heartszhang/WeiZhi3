using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using Weibo.Apis.SinaV2;
using Weibo.DataModel;
using Weibo.DataModel.Misc;

namespace Weibo.ViewModels
{
    public class Recomendations : ViewModelBase2
    {
        private static Random _random = new Random();
        private hot_status_type _type;
        private readonly List<WeiboStatus> _statuses;
        public List<WeiboStatus> statuses { get { return _statuses; } }
        public Recomendations()
        {
            _type = (hot_status_type) _random.Next(1, (int) hot_status_type.count);
            _statuses = new List<WeiboStatus>();
        }

        private int _tick;
        public async void OnTick(IWeiboAccessToken at)
        {
            if (_tick++%57 != 0)
                return;
            var resp = await WeiboClient.suggestions_statuses_hot_async(at.get(), 20, true, 1, _type);
            if(++_type == hot_status_type.count)
                _type = hot_status_type.musement;

            if (resp.Failed() || resp.Value.Length == 0)
                return;
            var queue = new PriorityQueue<long, Status>();
            foreach(var s in resp.Value)
            {
                queue.Enqueue(EvalMark(s),s);
            }
            while (queue.Size > 5)
                queue.Dequeue();
            while(!queue.IsEmpty)
            {
                var s = queue.Dequeue();
                var ws = new WeiboStatus();
                ws.assign_sina(s);
                await DispatcherHelper.UIDispatcher.BeginInvoke(DispatcherPriority.SystemIdle, (Action) (() =>
                {
                    if(statuses.Count > 10)
                        statuses.RemoveAt(0);
                    statuses.Add(ws);
                }));
            }
        }

        static long EvalMark(StatusWithoutUser s)
        {
            var text = s.text;
            var length = text.Split(new string[] { "//@" }, StringSplitOptions.RemoveEmptyEntries)[0].Length;
            var ats = text.Count((c) => c == '@');
            var hyperlinks = text.Length - text.Replace("http://t.cn/", string.Empty).Length;
            var topics = text.Count((c) => c == '#' || c == '《' || c == '【' || c == '『'
                                           || c == '<');

            long marks = length * 100;
            if (ats > 0 && topics > 0 && hyperlinks > 0)
                marks -= 3300;//这就是条广告
            if (ats > 2)
                marks -= ats*800;//更像一条广告了
            if (hyperlinks/10 > 1)
                marks -= 40;
            if (hyperlinks/10 > 0 && topics > 1)
                marks -= 300;
            marks *= (1+(s.comments_count + s.reposts_count/3)/60);

            return marks;
        }
    }
}