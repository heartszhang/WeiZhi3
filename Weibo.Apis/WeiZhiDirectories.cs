using System;
using System.IO;
using System.Linq;

namespace Weibo.Apis
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

        static string PicPath(string catalog)
        {
            return GetCreatePath(catalog);
        }

        public static string CacheFilePath(string catalog, string url, string ext)
        {
            var path = PicPath(catalog);
            var fn = HashFileName(url);
            return Path.Combine(path, fn + ext);
        }
        internal static ulong Hash(string url)
        {
            return url.Aggregate<char, ulong>(0, (current, i) => current * 47 + i);
        }
        internal static ulong Hash2(string url)
        {
            return url.Aggregate<char, ulong>(0, (current, i) => current * 41 + i);
        }

        public static string HashFileName(string online)
        {
            const string scheme = "http://";
            var idx = online.IndexOf(scheme, StringComparison.Ordinal);
            if (idx < 0)
                return null;
            var picv = Hash(online);
            var picv2 = Hash2(online);
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