using System.Runtime.Serialization;

namespace Weibo.DataModel
{
    [DataContract]
    public class WidgetShow
    {
        [DataMember]
        public string result { get; set; }
    }

}