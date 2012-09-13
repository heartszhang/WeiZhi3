using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;

namespace WeiZhi3.Behaviours
{
    sealed class WeiboAuthorizeBehavior : Behavior<Panel>
    {
        private const long ClientId = 164653724;

        private const string WeiboAuthorizehUrl
            = "https://api.weibo.com/oauth2/authorize";
        private const string AuthorizeUrlPattern
            = "{0}?client_id={1}&response_type=token&redirect_uri={2}";

        private const string Callback
            = "http://metroweili.sinaapp.com/callback";

        static Uri AuthorizeUrl()
        {
            return new Uri(string.Format(AuthorizeUrlPattern, WeiboAuthorizehUrl, ClientId, Callback), UriKind.Absolute);
        }
        protected override void OnAttached()
        {
            base.OnAttached();
            var wb = new WebBrowser();
            AssociatedObject.Children.Add(wb);

            wb.Navigating += OnWebNavigating;
            wb.Navigate(AuthorizeUrl());
        }

        static string GetAbsoluteUrl(Uri url)
        {
            return url.Scheme + "://" + url.Host
                   + (url.IsDefaultPort ? string.Empty : (":" + url.Port)) + url.AbsolutePath;
        }
        /*
         * https://api.weibo.com...
         * res://ieframe.dll...
         * http://metroweili.sinaapp.com/...
         * any links in the api.weibo.com are denied
         * send access-token by dialog message with 'authorize' token
         */
        void OnWebNavigating(object sender, NavigatingCancelEventArgs e)
        {
            Debug.WriteLine(e.Uri.AbsoluteUri);
            if (e.Uri.Scheme != "res" && e.Uri.Scheme != "javascript"
                && GetAbsoluteUrl(e.Uri) != WeiboAuthorizehUrl
                && GetAbsoluteUrl(e.Uri) != Callback)
            {
                e.Cancel = true;
                return;
            }
            if (string.IsNullOrEmpty(e.Uri.Fragment) || e.Uri.AbsoluteUri.Replace(e.Uri.Fragment, string.Empty) != Callback)
                return;
            var wb = sender as WebBrowser;
            Debug.Assert(wb != null);

            Messenger.Default.Send(
                new DialogMessage(
                    e.Uri.Fragment.Remove(0, 1), mbr =>
                                       {
                                           if (mbr == MessageBoxResult.Yes)
                                           {//重新进入请求认证流程
                                               e.Cancel = true;
                                               wb.Navigate(AuthorizeUrl());
                                           }
                                           else//成功认证，或者程序退出
                                           {
                                               e.Cancel = true;
                                               wb.Navigating -= OnWebNavigating;
                                               wb.NavigateToString("<html/>");
                                           }
                                       }), "authorize");
        }
    }
}