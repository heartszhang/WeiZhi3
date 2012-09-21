namespace Weibo.DataModel
{
    public enum WeiboSourcesType
    {
        Sina = 0,
        Tencent = 1,
        SinaV1 = 2,
    }
    public class WeiboSources
    {
        public static string SinaV2(string path)
        {
            return "https://api.weibo.com/2/" + path;
        }

        public static string SinaV1(string path)
        {
            return "https://api.weibo.com/" + path;
        }

        public static string SinaUpload(string path)
        {
            return "https://upload.api.weibo.com/2/" + path;
        }
    }
}