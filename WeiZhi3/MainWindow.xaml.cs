using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using WeiZhi3.ViewModel;
using WeiZhi3.Views;
using Weibo.Apis.SinaV2;
using Weibo.DataModel;
using Weibo.DataModel.Misc;
using Weibo.ViewModels;
using Weibo.ViewModels.DataModels;

namespace WeiZhi3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        private Timer _state_tick;
        public MainWindow()
        {
            InitializeComponent();
        }

        void NavigationWindowUnoaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= NavigationWindowUnoaded;
            if(_state_tick != null)
            {                
                _state_tick.Dispose();
                _state_tick = null;
            }
        }

        private async void NavigationWindowLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= NavigationWindowLoaded;

            _state_tick = new Timer(OnStateTick, null, 5000, 5000);

            /*
             处理授权页面发出的消息，*/
            Messenger.Default.Register<DialogMessage>(
                this, "authorize",
                OnAuthorizingMessage);

            /*检验授权是否有效*/
            var locator = (ViewModelLocator)FindResource("Locator");
            Debug.Assert(locator != null);
            await locator.Profile.VerifyAccounts().ContinueWith(
                (t)=>
                    {
                        Dispatcher.Invoke(
                            DispatcherPriority.SystemIdle
                            , (Action)(() => Navigate(locator.Profile.IsEmpty()
                                                           ? new Uri("/Pages/PageAuthorizing.xaml", UriKind.Relative)
                                                           : new Uri("/Pages/PageHome.xaml", UriKind.Relative))));
                    });
        }

        private void OnStateTick(object state)
        {
            
        }
        void UiInvoke(Action act)
        {
            Dispatcher.Invoke(DispatcherPriority.SystemIdle, act);
        }

        private void OnAuthorizingMessage(DialogMessage msg)
        {
            var rlt = MessageBoxResult.No;
            var r = ExtractAccessToken(msg.Content);
            if (r.Id == 0) //application error or user denied
            {
                rlt = DialogMessageBox.Show(string.Format("原因 {0} : {1}. {2}\r\n是：重新请求授权；否：返回", r.ErrorCode, r.Error, r.ErrorDescription), "重新尝试一次？", MessageBoxButton.YesNo, MessageBoxResult.No);
                if (rlt != MessageBoxResult.Yes)
                {
                    if (CanGoBack)
                        GoBack();
                    else Navigate(new Uri("/Pages/PageBootstrap.xaml", UriKind.Relative));
                }
            }
            else
            {                
                var l = (ViewModelLocator) FindResource("Locator");
                l.Profile.Add(r.AccessToken, r.ExpiresIn, r.Id);
                l.Profile.Save();
                //如果直接跳转页面会导致，回调页面在ie中打开
                Navigate(new Uri("/Pages/PageHome.xaml?id=" + r.Id, UriKind.Relative), r.Id);
            }
            msg.ProcessCallback(rlt);
        }

        private static AuthroizeResult ExtractAccessToken(string namevalues)
        {
            var rtn = new AuthroizeResult();
            var nvs = namevalues.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            if (nvs.Length == 0)
                 return rtn;
            
            foreach (var nv in nvs)
            {
                var namev = nv.Split(new char[] { '=' });
                if (namev.Length != 2)
                    continue;
                var val = Uri.UnescapeDataString(namev[1]);
                if (namev[0] == "access_token")
                    rtn.AccessToken = val;
                else if (namev[0] == "error")
                    rtn.Error = val;
                else if (namev[0] == "error_description")
                    rtn.ErrorDescription = val;
                else if (namev[0] == "error_code")
                    rtn.ErrorCode = int.Parse(val);
                else if (namev[0] == "expires_in")
                    rtn.ExpiresIn = DateTime.Now.AddSeconds(int.Parse(val)).UnixTimestamp();
                else if (namev[0] == "remind_in")
                    rtn.RemindIn = DateTime.Now.AddSeconds(int.Parse(val)).UnixTimestamp();
                else if (namev[0] == "uid")
                    rtn.Id = long.Parse(val);
            }

            return rtn;
        }
        UrlInfo UrlInfo(string url)
        {
            var mem = MemoryCache.Default;
            return (UrlInfo)mem.Get(url);
        }

        private void ExecuteWeiZhiCommandsNavigateUrl(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var url = (string) e.Parameter;
            var psi = new ProcessStartInfo(url) { UseShellExecute = true };
            using (Process.Start(psi))
            {
            }
        }
        private void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            var uri = e.Uri;
            if (uri == null || !uri.IsAbsoluteUri)
                return;
            var scheme = uri.Scheme;
            if(scheme.Contains("http"))
            {
                e.Cancel = true;
                var ui = UrlInfo(uri.AbsoluteUri);
                if(ui != null)
                {
                    if(ui.type == UrlType.Music)
                    {
                        MediaCommands.Play.Execute(ui,this);
                        return;
                    }
                    if(ui.type == UrlType.Video && ui.annotations != null && ui.annotations.Length > 0)
                    {
                        WeiZhiCommands.PlayVideo.Execute(ui,this);
                        return;
                    }
                }
                var psi = new ProcessStartInfo(uri.AbsoluteUri) {UseShellExecute = true};
                using(Process.Start(psi))
                {
                }
            }
        }

        private void ExecuteMediaCommandsPlay(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var ui = (UrlInfo) e.Parameter;
            Debug.Assert(ui != null);
            if (ui.type == UrlType.Music)
            {
                WeiZhiCommands.PlayMusic.Execute(ui,this);
                return;
            }
            if (ui.type == UrlType.Video && ui.annotations != null && ui.annotations.Length > 0)
            {
                WeiZhiCommands.PlayVideo.Execute(ui, this);
                return;
            }
        }

        private void ExecuteWeiZhiCommandsCreateTweet(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var wnd = new WeiboEditWindow() {Owner = this, DataContext = new WeiboEditViewModel(),ShowInTaskbar = false};
            wnd.Show();
        }

        private void ExecuteWeiZhiCommandsCopyName(object sender, ExecutedRoutedEventArgs e)
        {
            var name = (string) e.Parameter;
            Clipboard.SetText("@"+name);
            e.Handled = true;
        }

        private void ExecuteWeiZhiCommandsCopyTweet(object sender, ExecutedRoutedEventArgs e)
        {
            var ws = (WeiboStatus) e.Parameter;
            var txt = "//@" + ws.user.screen_name + ": " + ws.text;
            Clipboard.SetText(txt);
            e.Handled = true;
        }

        private void ExecuteWeiZhiCommandsViewUserViaWeb(object sender, ExecutedRoutedEventArgs e)
        {
            var user = (UserExt) e.Parameter;
            var url = "http://weibo.com/u/" + user.id;
            var psi = new ProcessStartInfo(url) { UseShellExecute = true };
            using (Process.Start(psi))
            {
            }
            e.Handled = true;
        }

        private async void ExecuteWeiZhiCommandsUnfollow(UserExt user, string token)
        {
            var resp = await WeiboClient.friendships_destroy_async(user.id, token);
            var caption = string.Format("取消关注 @{0}", user.screen_name);
            DialogMessageBox.Show(resp.Failed() ? resp.Reason : "成功", caption, MessageBoxButton.OK, MessageBoxResult.OK);
            if (!resp.Failed())
                user.following = false;
        }

        private async void ExecuteWeiZhiCommandsFollow(UserExt user, string token)
        {
            var resp = await WeiboClient.friendships_create_async(user.id, token);
            var caption = string.Format("关注 @{0}", user.screen_name);
            DialogMessageBox.Show(resp.Failed() ? resp.Reason : "成功", caption, MessageBoxButton.OK, MessageBoxResult.OK);
            if (!resp.Failed())
                user.following = true;
        }

        private void ExecuteWeiZhiCommandsFollowUnfollow(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var user = (UserExt)e.Parameter;
            if (!user.following)
                ExecuteWeiZhiCommandsFollow(user,Token().get());
            else
            {
                ExecuteWeiZhiCommandsUnfollow(user, Token().get());
            }
        }
        IWeiboAccessToken Token()
        {
            var token = ((ViewModelLocator)FindResource("Locator")).AccessToken;
            return token;
        }
        private async void ExecuteWeiZhiCommandsRetweetDirectly(object sender, ExecutedRoutedEventArgs e)
        {
            var ws = (WeiboStatus) e.Parameter;
            var resp = await WeiboClient.statuses_repost_async(string.Empty, ws.id,
                                                         WeiboClient.StatusesRepostIsCommentFlag.NoComment,
                                                         Token().get());
            if (resp.Failed())
                DialogMessageBox.Show(resp.Reason, string.Format("转发 @{0} 的微博", ws.user.screen_name),
                                      MessageBoxButton.OK, MessageBoxResult.OK);
            else
            {
                Messenger.Default.Send(new NotificationMessage(string.Format("已经转发 @{0} 的微博", ws.user.screen_name)), "noti");
            }
        }
    }
}
