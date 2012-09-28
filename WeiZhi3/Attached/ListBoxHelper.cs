using System.Windows;
using Weibo.ViewModels;

namespace WeiZhi3.Attached
{
    internal class ListBoxHelper
    {

        #region MouseOverItem (Attached DependencyProperty)

        public static readonly DependencyProperty MouseOverItemProperty =
            DependencyProperty.RegisterAttached("MouseOverItem", typeof(WeiboStatus), typeof(ListBoxHelper), new PropertyMetadata(new PropertyChangedCallback(OnMouseOverItemChanged)));

        public static void SetMouseOverItem(DependencyObject o, WeiboStatus value)
        {
            o.SetValue(MouseOverItemProperty, value);
        }

        public static WeiboStatus GetMouseOverItem(DependencyObject o)
        {
            return (WeiboStatus)o.GetValue(MouseOverItemProperty);
        }

        private static void OnMouseOverItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

    }
}