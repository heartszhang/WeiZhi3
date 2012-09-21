using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Weibo.Apis.Net
{
    internal static class ResponseParser
    {
        public static TResult Extract<TResult>(byte[] resp) where TResult : class
        {
            if (resp == null || resp.Length == 0)
                return default(TResult);
            var ser = new DataContractJsonSerializer(typeof(TResult));
            using (var ms = new MemoryStream(resp))
            {
                return ser.ReadObject(ms) as TResult;
            }
        }
        public static TResult Extract<TResult>(string resp) where TResult : class
        {
            if (string.IsNullOrEmpty(resp))
                return default(TResult);
            return Extract<TResult>(Encoding.UTF8.GetBytes(resp));
        }
    }
}
