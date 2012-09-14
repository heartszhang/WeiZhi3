using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WeiZhi.Base
{
    public class DesString
    {
        const string Key = "wX7n*y2a";

        public static string EncryptQueryString(string str)
        {
            byte[] key = { };
            byte[] iv = { 0x01, 0x12, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78 };
            key = Encoding.UTF8.GetBytes(Key);
            var input = Encoding.UTF8.GetBytes(str);
            using (var desprovider = new DESCryptoServiceProvider())
            {
                using (var ms = new MemoryStream())
                {
                    using (var cryptstream = new CryptoStream(ms,
                        desprovider.CreateEncryptor(key, iv),
                        CryptoStreamMode.Write))
                    {
                        cryptstream.Write(input, 0, input.Length);
                        cryptstream.FlushFinalBlock();
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }
        // Decrypt a Querystring
        public static string DecryptQueryString(string str)
        {
            byte[] key = { };
            byte[] iv = { 0x01, 0x12, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78 };
            str = str.Replace(" ", "+");


            key = Encoding.UTF8.GetBytes(Key);
            using (var des = new DESCryptoServiceProvider())
            {
                byte[] input = Convert.FromBase64String(str);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Write))
                    {
                        cs.Write(input, 0, input.Length);
                        cs.FlushFinalBlock();
                        var encoding = Encoding.UTF8;
                        return encoding.GetString(ms.ToArray());
                    }
                }
            }
        }
    }
}