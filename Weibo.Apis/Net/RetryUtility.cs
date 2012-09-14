using System;

namespace Weibo.Apis.Net
{
    public static class RetryUtility
    {
        public static void RetryAction(Action action, int numRetries=2)
        {
            if (action == null)
                throw new ArgumentNullException("action"); // slightly safer...

            do
            {
                try { action(); return; }
                catch
                {
                    if (numRetries <= 0) throw;  // improved to avoid silent failure
                }
            } while (numRetries-- > 0);
        }
    }
}