using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
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
using Weibo.DataModel;
using Weibo.ViewModels;

namespace WeiZhi3.Parts
{
    /// <summary>
    /// Interaction logic for WeiboRetweetControl.xaml
    /// </summary>
    public partial class WeiboRetweetControl : UserControl
    {
        public WeiboRetweetControl()
        {
            InitializeComponent();
        }
        private void OnMouseLeftButtonDownImp(object sender, MouseButtonEventArgs e)
        {
            var vm = (WeiboStatus)DataContext;
            vm.show_editor.Execute(null);
        }

        private void ExecuteWeiZhiCommandsNavigate(object sender, ExecutedRoutedEventArgs e)
        {
            var mem = MemoryCache.Default;
            var ui = (UrlInfo)mem.Get((string) e.Parameter);
            e.Handled = true;
            if (ui != null && ui.has_document())
            {
                var ws = (WeiboStatus) DataContext;
                ws.reverse_document_ready();
            }
            else
            {
                var wnd = Window.GetWindow(this) as NavigationWindow;
                Debug.Assert(wnd != null);
                wnd.NavigationService.Navigate(new Uri((string) e.Parameter));
            }
        }
    }
}
