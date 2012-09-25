using System;
using System.Windows.Controls;

namespace WeiZhi3.Parts
{
    /*如果当前微博滚动了好几屏，为了保证图片收回的时候，还能够看见当前这个微博，需要把这条微博重新显示出来*/
    internal class ListBoxExt :ListBox
    {
        private ScrollViewer _scroll;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scroll = (ScrollViewer)Template.FindName("_scroll", this);
            _scroll.ScrollChanged += ScrollChanged;
            Unloaded += ListBoxExtUnloaded;
        }

        void ListBoxExtUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _scroll.ScrollChanged -= ScrollChanged;
            Unloaded -= ListBoxExtUnloaded;
        }

        void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (Math.Abs(e.ExtentHeightChange) < double.Epsilon)
                return;
            if (Math.Abs(e.ViewportHeightChange) + Math.Abs(e.ViewportWidthChange) > double.Epsilon)
                return;//窗口大小发生变化，不进行处理
            if (Math.Abs(e.ExtentWidthChange) > double.Epsilon)//宽度也发生变化
                return;
            if (e.ExtentHeightChange > 0)//长度变大，也不处理
                return;
            Dispatcher.BeginInvoke((Action)(() => ScrollIntoView(SelectedItem)));
        }
    }
}