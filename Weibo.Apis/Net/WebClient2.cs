using System;
using System.Net;

namespace Weibo.Apis.Net
{
    internal class WebClient2 : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).KeepAlive = true;
            }
            return request;
        }
        public void Retry(Action<WebClient> action, int numRetries = 2)
        {
            if (action == null)
                throw new ArgumentNullException("action"); // slightly safer...

            do
            {
                try { action(this); return; }
                catch (WebException)
                {
                    if (numRetries <= 0) throw;  // improved to avoid silent failure
                }
            } while (numRetries-- > 0);
        }
    }
}