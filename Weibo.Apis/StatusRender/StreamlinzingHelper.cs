using System;
using System.Linq;

namespace Weibo.Apis.StatusRender
{
    public static class StreamlinzingHelper
    {
        internal static string ExtractChars(string t)
        {
            var rtn = t.Where(i => !i.IsPunctuationExt()).Aggregate(string.Empty, (current, i) => current + i);
            return rtn;
        }
        internal static int punctuation_count(string txt)
        {
            return txt.Count(char.IsPunctuation);
        }

        public static string Streamline(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            var i = text.IndexOf("//@", StringComparison.Ordinal);
            var t = i > -1 ? text.Substring(0, i) : text;
            return ExtractChars(t.Replace("转发微博", string.Empty));
        }
        public static string RemovePunctuations(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            var ridx = 0;
            foreach (var c in s.Reverse())
            {
                if ((c.IsPunctuationExt() || char.IsWhiteSpace(c))
                    && c != ']' && c != '>' && c != '》' && c != '”' && c != '"')
                    ++ridx;
                else break;
            }

            return s.Substring(0, s.Length - ridx);

        }
    }
}