using System;
using System.IO;
using System.Linq;

namespace WeiZhi.Base
{
    public static class WeiZhiDirectories
    {
        public static string UserRoot()
        {
            var r = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var root = r + @"\WeiZhi\";
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            return root;
        }
        static string GetCreatePath(string relative)
        {
            var path = Path.Combine(UserRoot(), relative);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
        internal static string HtmlPath()
        {
            return GetCreatePath(@"htmls\");
        }
        static string PicPath(string catalog)
        {
            return GetCreatePath(catalog + '\\');
        }
        internal static string AvatarCachePath()
        {
            return PicPath("avatars");
        }
        internal static string ThumbnailCachePath()
        {
            return PicPath("thumbnails");
        }
        internal static string MiddleCachePath()
        {
            return PicPath("middles");
        }
        internal static string CacheFilePath(string catalog, string url, string ext)
        {
            var path = PicPath(catalog);
            var fn = hash_file_name(url);
            return path + fn + ext;
        }
        internal static ulong hash(string url)
        {
            return url.Aggregate<char, ulong>(0, (current, i) => current * 47 + i);
        }
        internal static ulong hash2(string url)
        {
            return url.Aggregate<char, ulong>(0, (current, i) => current * 41 + i);
        }

        public static string hash_file_name(string online)
        {
            const string scheme = "http://";
            var idx = online.IndexOf(scheme, StringComparison.Ordinal);
            if (idx < 0)
                return null;
            var picv = hash(online);
            var picv2 = hash2(online);
            var p = string.Empty;
            for(var i = idx + scheme.Length; i< online.Length ; ++i)
            {
                if (char.IsLetterOrDigit(online[i]))
                    p += online[i];
            }
            return @"hash-" + p + '-' + picv + '-' + picv2;
        }
    }
}