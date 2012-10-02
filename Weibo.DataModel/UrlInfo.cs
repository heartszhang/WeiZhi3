using System;
using System.Linq;
using System.Runtime.Serialization;
using Weibo.DataModel.Misc;

namespace Weibo.DataModel
{
    [DataContract]
    public class UrlInfo
    {
        [DataMember]
        public string url_short { get; set; }
        [DataMember]
        public string url_long { get; set; }
        [DataMember]
        public UrlType type { get; set; }
        [DataMember]
        public bool result { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string last_modified { get; set; }

        [DataMember]
        public UrlInfoAnnotations[] annotations { get; set; }

        public string topic()
        {
            if (annotations != null &&annotations.Length > 0
                && !string.IsNullOrEmpty(annotations[0].title) && string.IsNullOrEmpty(title))
                title = annotations[0].title;
            var rtn = title;
            if (string.IsNullOrEmpty(title))
                return rtn;
            var a1 = title.Split(new char[] { '|', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (a1.Length > 1)
            {
                rtn = a1[0];
            }
            else
            {
                var b1 = title.Split(new char[] { ':', '£º' }, StringSplitOptions.RemoveEmptyEntries);
                if (b1.Length > 1)
                    rtn = b1.Last();
            }
            if (rtn.Length < 5)
                rtn = title;
            return rtn;
        }
        public string relative_short()
        {
            return url_short.Remove(0, "http://t.cn/".Length);
        }
        public bool has_document() 
        {
            return type == UrlType.Normal || type == UrlType.News || type == UrlType.Blog; 
        }

    }
}