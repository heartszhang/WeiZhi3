using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
using Weibo.Api2;
using Weibo.Api2.Sina;
using Weibo.ViewModels.DataModels;

namespace WeiZhi3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void NavigationWindowLoaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<DialogMessage>(
                this, "authorize",
                msg =>
                {
                    var rlt = MessageBoxResult.No;
                    var r = ExtractAccessToken(msg.Content);
                    if (r.Id == 0)//application error or user denied
                    {
                        rlt = DialogMessageBox.Show(string.Format("原因 {0} : {1}. {2}\r\n是：重新请求授权；否：返回", r.ErrorCode, r.Error, r.ErrorDescription)
                            , "重新尝试一次？", MessageBoxButton.YesNo, MessageBoxResult.No);
                        if(rlt != MessageBoxResult.Yes)
                        {
                            if (CanGoBack)
                                GoBack();
                            else Navigate(new Uri("/Pages/PageBootstrap.xaml", UriKind.Relative));
                        }
                    }
                    else
                    {
                        var l = (ViewModelLocator)FindResource("Locator");
                        l.Profile.Add(r.AccessToken, r.ExpiresIn, r.Id);
                        l.Profile.Save();
                        //如果直接跳转页面会导致，回调页面在ie中打开
                        Navigate(new Uri("/Pages/PageHome.xaml", UriKind.Relative), r.Id);
                    }
                    msg.ProcessCallback(rlt);
                });

            var locator = (ViewModelLocator)FindResource("Locator");
            Debug.Assert(locator != null);
            await locator.Profile.VerifyAccounts();
            Dispatcher.Invoke(
                DispatcherPriority.SystemIdle
                , (Action) (() => Navigate(locator.Profile.IsEmpty()
                                               ? new Uri("/Pages/PageAuthorizing.xaml", UriKind.Relative)
                                               : new Uri("/Pages/PageHome.xaml", UriKind.Relative))));
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
    }
}
