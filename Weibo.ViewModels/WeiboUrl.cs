using Weibo.DataModel;
using Weibo.DataModel.Misc;

namespace Weibo.ViewModels
{
    public class WeiboUrl : ObservableObjectExt
    {
        private UrlInfo _data = new UrlInfo();
        private string _music;
        public void assign(UrlInfo ui)
        {
            _data = ui;
        }
        bool has_annotations { get { return _data != null && _data.annotations != null && _data.annotations.Length > 0; } }
        public string topic 
        {
            get { return _data.topic(); } 
        }//from urlinfo or it's annotations

        public string short_path { get { return _data.url_short.Remove(0, 12); } }//not include http://t.cn/.Length == 12

        public bool has_document { get { return _data.type == UrlType.Normal || _data.type == UrlType.News || _data.type == UrlType.Blog; } }
        public bool has_music { get { return _data.type == UrlType.Music; } }
        public bool has_video { get { return _data.type == UrlType.Video; } }
        public bool has_media
        {
            get
            {
                return has_music || has_video;
            }
        }

        public UrlInfo data { get { return _data; } }//used for media play
        public UrlType type { get { return _data.type; } }

        public string pic { get { return has_annotations ? _data.annotations[0].pic : null; } }
        public string author { get { return has_annotations ? _data.annotations[0].author : null; } }
        public string url_long { get { return _data.url_long; } }
        public string video{ get { return has_annotations && has_video ? _data.annotations[0].url : null; } }//video url
        public string music { get { return _music; } set { Set(ref _music, value); } }//需要初始化过程中获取
        public string document { get { return has_document ? _data.url_short : null; } }//通过这个才能转化成flowdocument
        public string album { get { return has_annotations ? _data.annotations[0].album : null; } }
        public string mp4 { get { return has_annotations ? _data.annotations[0].mp4 : null; } }//used for sina video
    }
}