using System.Diagnostics;
using System.Runtime.Caching;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;
using WeiZhi3.Converters;
using WeiZhi3.ViewModel;
using Weibo.DataModel;
using Weibo.ViewModels;

namespace WeiZhi3.Parts
{
    public partial class WeiboControlBase : UserControl
    {
//        private MenuItem _follow_current;
        //private MenuItem _follow_retweet;

        protected void OnMouseLeftButtonDownImp(object sender, MouseButtonEventArgs e)
        {
            var vm = (WeiboStatus)DataContext;
            vm.show_editor.Execute(null);
        }

        protected void ExecuteWeiZhiCommandsNavigate(object sender, ExecutedRoutedEventArgs e)
        {
            var mem = MemoryCache.Default;
            var ui = (UrlInfo)mem.Get((string)e.Parameter);
            e.Handled = true;
            if (ui != null && ui.has_document())
            {
                var ws = (WeiboStatus)DataContext;
                //ws.is_document_ready = true;
                ws.reverse_document_ready();
            }
            else
            {
                var wnd = Window.GetWindow(this) as NavigationWindow;
                Debug.Assert(wnd != null);
                wnd.NavigationService.Navigate((string)e.Parameter);
            }
        }

        protected void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var uc = (UserControl)sender;
            var cm = uc.ContextMenu;
            var ws = (WeiboStatus)DataContext;
            if (cm.Items.Count != 0)//all are ready
            {
                return;
            }

            cm.Items.Add(new MenuItem()
            {
                Header = "复制到剪贴板",
                Command = WeiZhiCommands.CopyTweet,
                CommandParameter = ws
            });
            cm.Items.Add(new MenuItem()
            {
                Header = string.Format("复制 @{0} 到剪贴板", ws.user.screen_name),
                Command = WeiZhiCommands.CopyName,
                CommandParameter = ws.user.screen_name
            });
            if (ws.has_rt)
            {
                cm.Items.Add(new MenuItem
                {
                    Header = "复制原推内容到剪贴板",
                    Command = WeiZhiCommands.CopyTweet,
                    CommandParameter = ws.retweeted_status
                });
                cm.Items.Add(new MenuItem
                {
                    Header = string.Format("复制 @{0} 到剪贴板", ws.retweeted_status.user.screen_name),
                    Command = WeiZhiCommands.CopyName,
                    CommandParameter = ws.retweeted_status.user.screen_name
                });
            }

            cm.Items.Add(new Separator());
            //cm.Items.Add(new MenuItem
            //{
            //    Header = "在浏览器中查看此微博",
            //    Command = WeiZhiCommands.ViewWeiboViaWeb,
            //    CommandParameter = ws
            //});
            cm.Items.Add(new MenuItem { Header = string.Format("在浏览器中查看 @{0}", ws.user.screen_name), Command = WeiZhiCommands.ViewUserViaWeb, CommandParameter = ws.user });
            if (ws.has_rt)
                cm.Items.Add(new MenuItem { Header = string.Format("在浏览器中查看 @{0}", ws.retweeted_status.user.screen_name), Command = WeiZhiCommands.ViewUserViaWeb, CommandParameter = ws.retweeted_status.user });
            if (!ws.url.is_empty)
            {
                if (ws.url.has_document)
                    cm.Items.Add(new MenuItem { Header = string.Format("在浏览器中查看 {0}", ws.url.short_path) , Command = WeiZhiCommands.NavigateUrl, CommandParameter = ws.url.url_short });
                if (ws.url.has_media)
                    cm.Items.Add(new MenuItem { Header = string.Format("在浏览器中播放 {0}", ws.url.short_path), Command = WeiZhiCommands.NavigateUrl, CommandParameter = ws.url.url_short });
            }
            if (ws.has_pic)
                cm.Items.Add(new MenuItem { Header = "打开图片", Command = WeiZhiCommands.NavigateUrl, CommandParameter = ws.bmiddle_pic });

            cm.Items.Add(new Separator());
            var fc = new MenuItem()
            {
                Command = WeiZhiCommands.FollowUnfollow,
                CommandParameter = ws.user
            };
            var bd = new Binding("user.following") { Converter = new FollowCommandConverter(ws.user.screen_name), Mode = BindingMode.OneWay };
            fc.SetBinding(HeaderedItemsControl.HeaderProperty, bd);
            cm.Items.Add(fc);

            if (ws.has_rt && ws.user.id != ws.retweeted_status.user.id)
            {
                var ff = new MenuItem
                {
                    Command = WeiZhiCommands.FollowUnfollow,
                    CommandParameter = ws.retweeted_status.user
                };
                var rbd = new Binding("retweeted_status.user.following") { Converter = new FollowCommandConverter(ws.retweeted_status.user.screen_name),Mode=BindingMode.OneWay };
                ff.SetBinding(HeaderedItemsControl.HeaderProperty, rbd);
                cm.Items.Add(ff);
                
            }
            cm.Items.Add(new Separator());
            cm.Items.Add(new MenuItem { Header = "转发/评论", Command = ((WeiboStatus)DataContext).show_editor, CommandParameter = ws });
            cm.Items.Add(new MenuItem { Header = "直接转发", Command = WeiZhiCommands.RetweetDirectly, CommandParameter = ws });

            var l = (ViewModelLocator)FindResource("Locator");
            var cid = l.AccessToken.id();
            if (cid == ws.user.id)
                cm.Items.Add(new MenuItem { Header = "删除", Command = WeiZhiCommands.DeleteTweet, CommandParameter = ws });
            if (ws.has_rt)
            {
                cm.Items.Add(new MenuItem
                {
                    Header = string.Format("转发 @{0}的原微博", ws.retweeted_status.user.screen_name),
                    Command = WeiZhiCommands.Retweet,
                    CommandParameter = ws.retweeted_status
                });
                cm.Items.Add(new MenuItem
                {
                    Header = string.Format("评论 @{0}的原微博", ws.retweeted_status.user.screen_name),
                    Command = WeiZhiCommands.Reply,
                    CommandParameter = ws.retweeted_status,
                });
            }

        }
    }
}