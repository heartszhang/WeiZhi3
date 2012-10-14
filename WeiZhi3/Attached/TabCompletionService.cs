using System.Collections.Generic;
using System.Linq;

namespace WeiZhi3.Attached
{
    class TabCompletionService
    {
        protected readonly Dictionary<string, HashSet<string>> _entries = new Dictionary<string, HashSet<string>>();
        public string Suffix(string prefix)
        {
            if (_entries.ContainsKey(prefix))
                return _entries[prefix].First();
            return string.Empty;
        }
        public string SuffixNext(string prefix, string sufix, bool cycling)
        {
            if (string.IsNullOrEmpty(prefix))
                return Suffix(prefix);
            if (!_entries.ContainsKey(prefix))
                return string.Empty;
            var vals = _entries[prefix];
            if (vals.Count < 2)
                return string.Empty;
            var items = vals.ToList();
            var idx = items.IndexOf(sufix) + 1;
            if (idx < items.Count)
                return items[idx];
            return cycling ? items[0] : string.Empty;
        }
        public TabCompletionService()
        {
        }
        protected internal List<string> Suffixes(string prefix)
        {
            if (!_entries.ContainsKey(prefix))
                return new List<string>();
            return _entries[prefix].ToList();
        }
        public void Add(string prefix, string suffix)
        {
            HashSet<string> suffixes;
            if (!_entries.TryGetValue(prefix, out suffixes))
                _entries.Add(prefix, suffixes = new HashSet<string>());
            suffixes.Add(suffix);
        }
        public void AddWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return;
            for (var i = 1; i < word.Length - 1; ++i)
            {
                var prefix = word.Substring(0, i);
                var suffix = word.Substring(i);
                Add(prefix, suffix);
            }
        }
    }
}