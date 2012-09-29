using System.Runtime.Serialization;
using System.Xml;

namespace Weibo.DataModel
{
    [DataContract]
    public class WidgetShow
    {
        [DataMember]
        public string result { get; set; }
        public string mp4()
        {
            if (string.IsNullOrEmpty(result))
                return null;
            var doc = new XmlDocument { XmlResolver = null };
            doc.LoadXml(result.Replace(@"&", @"&amp;"));//��֪��Ϊʲô��Ҫת�������&,�����������Ҫת��
            var param = doc.SelectSingleNode(@"//video/@src") as XmlAttribute;
            return param != null ? param.Value : null;
        }
    }

}