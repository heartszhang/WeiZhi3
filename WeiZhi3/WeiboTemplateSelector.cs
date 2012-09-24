using System.Windows;
using System.Windows.Controls;
using WeiZhi3.Parts;
using Weibo.ViewModels;

namespace WeiZhi3
{
    internal sealed class WeiboTemplateSelector : DataTemplateSelector
    {
        static DataTemplate CreateDataTemplate<TWeiboControl>() where TWeiboControl : UserControl
        {
            var rtn = new DataTemplate { DataType = typeof(WeiboStatus) };
            var ui = new FrameworkElementFactory(typeof(TWeiboControl));

            rtn.VisualTree = ui;
            return rtn;
        }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var ws = item as WeiboStatus;
            if (ws == null)
                return base.SelectTemplate(item, container);
            if (ws.has_rt)
                return CreateDataTemplate<WeiboRetweetControl>();
            else return CreateDataTemplate<WeiboControl>();
        }
    }
}