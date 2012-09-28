using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class Comment
    {
        [DataMember]
        public string created_at { get; set; }

        [DataMember]
        public long id { get; set; }

        [DataMember]
        public string text { get; set; }

        [DataMember]
        public User user { get; set; }

        [DataMember]
        public Status status { get; set; }

        [DataMember]
        public string source { get; set; }

        [DataMember]
        public bool favorited { get; set; }

        [DataMember]
        public bool truncated { get; set; }

        [DataMember]
        public Comment reply_comment { get; set; }

        [DataMember]
        public long mid { get; set; }

        public void preprocess()
        {
            if (string.IsNullOrEmpty(text))
                return;
            var i = text.IndexOf("//@", StringComparison.Ordinal);
            var t = i > -1 ? text.Substring(0, i) : text;
            var x = t.Count(c => c == '@');            
            score = extract_chars(t.Replace("转发微博", string.Empty)).Length;
            score -= x*5;
        }

        public int score { get; set; }
        internal static string extract_chars(string t)
        {
            var rtn = t.Where(i => !char.IsPunctuation(i)).Aggregate(string.Empty, (current, i) => current + i);
            return rtn;
        }
  
    }
}