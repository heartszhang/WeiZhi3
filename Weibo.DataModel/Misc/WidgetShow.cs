using System.Runtime.Serialization;

namespace Weibo.DataModel.Misc
{
    [DataContract]
    public class WidgetShow
    {
        [DataMember]
        public string result { get; set; }
    }

}