using System;
using System.Windows.Controls;

namespace WeiZhi3.Parts
{
    /*�����ǰ΢�������˺ü�����Ϊ�˱�֤ͼƬ�ջص�ʱ�򣬻��ܹ�������ǰ���΢������Ҫ������΢��������ʾ����*/
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
                return;//���ڴ�С�����仯�������д���
            if (Math.Abs(e.ExtentWidthChange) > double.Epsilon)//���Ҳ�����仯
                return;
            if (e.ExtentHeightChange > 0)//���ȱ��Ҳ������
                return;
            Dispatcher.BeginInvoke((Action)(() => ScrollIntoView(SelectedItem)));
        }
    }
}