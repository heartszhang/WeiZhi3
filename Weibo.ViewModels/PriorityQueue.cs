using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Weibo.ViewModels
{
    class PriorityQueue<TP, TV>
    {
        private long _size;
        private readonly SortedDictionary<TP, Queue<TV>> _list = new SortedDictionary<TP, Queue<TV>>();
        public void Enqueue(TP priority, TV value)
        {
            Queue<TV> q;
            if (!_list.TryGetValue(priority, out q))
            {
                q = new Queue<TV>();
                _list.Add(priority, q);
            }
            q.Enqueue(value);
            Interlocked.Increment(ref _size);
        }
        public TV Dequeue()
        {
            // will throw if there isn¡¯t any first element!
            var pair = _list.First();
            var v = pair.Value.Dequeue();
            if (pair.Value.Count == 0) // nothing left of the top priority.
                _list.Remove(pair.Key);
            Interlocked.Decrement(ref _size);
            return v;
        }
        public bool IsEmpty
        {
            get { return !_list.Any(); }
        }
        public long Size
        {
            get { return Interlocked.Read(ref _size); }
        }
    }
}