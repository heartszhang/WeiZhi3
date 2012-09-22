using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using Weibo.DataModel;
using Weibo.DataModel.Misc;
using Weibo.ViewModels.StatusRender;

namespace Weibo.ViewModels
{
    public enum WeiboTopicSource
    {
        Reserved=0,
        FirstSentence,
        Quote,
        Trend ,
        Url,
        UrlContent,
        Retweeted,
    }
    public class WeiboStatus : ObservableObjectExt
    {
        public DateTime created_at { get; set; }
        public string text { get; set; }
        public long id { get; set; }
        public long mid { get; set; }

        public string thumbnail_pic
        {
            get;
            set;
        }
        public string bmiddle_pic { get; set; }
        public string original_pic { get; set; }

        public bool favorited { get; set; }

        public int comments_count
        {
            get;
            set;
        }
        public int reposts_count
        {
            get;
            set;
        }
        public int thumb_pic_width { get; set; }
        public int thumb_pic_height { get; set; }

        public string url_topic { get; set; }
        public string url_short { get; set; }
        public string url_long { get; set; }
        public int url_type { get; set; }

        public UserExt user { get; set; }
        public WeiboStatus retweeted_status { get; set; }

        #region

        public string topic { get; set; }
        public string widget { get; set; }

        public bool has_pic { get; set; }

        public WeiboTopicSource topic_source { get; set; }
        #endregion

        public List<Token> tokens { get; set; }

        public void assign_sina(StatusWithoutRt data, bool enable_post_initial)
        {
            /*
            idstr	string	字符串型的微博ID
            created_at	string	创建时间
            id	int64	微博ID
            text	string	微博信息内容
            source	string	微博来源
            favorited	boolean	是否已收藏
            truncated	boolean	是否被截断
            in_reply_to_status_id	int64	回复ID
            in_reply_to_user_id	int64	回复人UID
            in_reply_to_screen_name	string	回复人昵称
            mid	int64	微博MID
            bmiddle_pic	string	中等尺寸图片地址
            original_pic	string	原始图片地址
            thumbnail_pic	string	缩略图片地址
            reposts_count	int	转发数
            comments_count	int	评论数
            annotations	array	微博附加注释信息
            geo	object	地理信息字段
            user */
            //var jt = ((JToken)data.created_at);

            created_at = time(data.created_at);
            id = data.id;
            text = data.text;
            favorited = data.favorited;
            mid = data.mid;
            bmiddle_pic = data.bmiddle_pic;
            thumbnail_pic = data.thumbnail_pic;
            original_pic = data.original_pic;
            reposts_count = data.reposts_count;
            comments_count = data.comments_count;
            user = new UserExt();
            user.assign_sina(data.user);

            has_pic = !string.IsNullOrEmpty(bmiddle_pic);
            if(enable_post_initial)
                post_initialize();
        }
        public void assign_sina(Status data)
        {
            assign_sina(data,false);
            if(data.retweeted_status != null)
            {
                retweeted_status = new WeiboStatus();
                retweeted_status.assign_sina(data.retweeted_status,true);

                if(retweeted_status.has_pic && !has_pic)
                {
                    bmiddle_pic = retweeted_status.bmiddle_pic;
                    thumbnail_pic = retweeted_status.thumbnail_pic;

                    thumb_pic_width = retweeted_status.thumb_pic_width;
                    thumb_pic_height = retweeted_status.thumb_pic_height;
                }
            }
            post_initialize();

        }
        internal static DateTime time(string tm)
        {
            if (string.IsNullOrEmpty(tm))
                return DateTime.Now;
            const string format = "ddd MMM dd HH:mm:ss zzzz yyyy"; //"ddd MMM dd HH:mm:ss zzzz yyyy";
            var tmt = DateTime.ParseExact(tm, format, new CultureInfo("en-US", true));
            return tmt;
        }

        void post_initialize()
        {
            tokens = text.Parse();
            var f = tokens.ElementAtOrDefault(0);
            Debug.Assert(f != null);
            if(f.tag == WeiboTokenTypes.Part)
            {
                topic_source = WeiboTopicSource.FirstSentence;
                topic = f.text.Trim();
            }

            foreach(var t in tokens)
            {
                if (t.tag == WeiboTokenTypes.Topic)
                {
                    topic = t.text;
                    topic_source = WeiboTopicSource.Trend;
                    break;
                }
                if(t.tag == WeiboTokenTypes.Quote && topic_source == WeiboTopicSource.Reserved)
                {
                    topic_source = WeiboTopicSource.Quote;
                    topic = t.text.Trim();
                }
            }
            while (tokens.Count > 0)
            {
                if (tokens[0].tag == WeiboTokenTypes.Punctuation)
                    tokens.RemoveAt(0);
                else
                    break;
            }
            if(retweeted_status != null)
            {
                if(topic_source == WeiboTopicSource.Reserved && retweeted_status.topic_source != WeiboTopicSource.Reserved)
                {
                    topic_source = WeiboTopicSource.Retweeted;
                    topic = retweeted_status.topic;
                }
                else if(topic_source < WeiboTopicSource.FirstSentence && retweeted_status.topic_source >= WeiboTopicSource.Trend)
                {
                    topic_source = WeiboTopicSource.Retweeted;
                    topic = retweeted_status.topic;
                }
            }
        }
    }
}
