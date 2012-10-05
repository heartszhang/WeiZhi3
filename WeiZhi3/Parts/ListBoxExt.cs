using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WeiZhi3.Parts
{
    /*�����ǰ΢�������˺ü�����Ϊ�˱�֤ͼƬ�ջص�ʱ�򣬻��ܹ�������ǰ���΢������Ҫ������΢��������ʾ����*/
    internal class ListBoxExt :ListBox
    {
        public static readonly RoutedEvent LastPageEvent =
            EventManager.RegisterRoutedEvent("LastPage",
                                             RoutingStrategy.Bubble,
                                             typeof(RoutedEventHandler),
                                             typeof(
                                                 ListBoxExt
                                                 ));
        public event RoutedEventHandler LastPage
        {
            add { AddHandler(LastPageEvent, value); }
            remove { RemoveHandler(LastPageEvent, value); }
        }

        public object HoveredItem
        {
            get { return (object)GetValue(HoveredItemProperty); }
            set { SetValue(HoveredItemProperty, value); }
        }
        public static DependencyProperty HoveredItemProperty = DependencyProperty.Register("HoveredItem", typeof(object), typeof(ListBoxExt), null);
        
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
        public void Scroll(int alpha)
        {
            _scroll.ScrollToVerticalOffset(_scroll.VerticalOffset +alpha);
        }
        void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            CheckIsLastPage(e);
            if (Math.Abs(e.ExtentHeightChange) < double.Epsilon)
                return;
            if (Math.Abs(e.ViewportHeightChange) + Math.Abs(e.ViewportWidthChange) > double.Epsilon)
                return;//���ڴ�С�����仯�������д���
            if (Math.Abs(e.ExtentWidthChange) > double.Epsilon)//���Ҳ�����仯
                return;
            if (e.ExtentHeightChange > 0)//���ȱ��Ҳ������
                return;
            if(HoveredItem != null)
                Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,(Action)(() => ScrollIntoView(HoveredItem)));
        }

        private bool _is_in_last_page;
        void CheckIsLastPage(ScrollChangedEventArgs e)
        {
            if (_scroll == null)
                return;
            if (Math.Abs(e.ExtentHeightChange) + Math.Abs(e.ExtentWidthChange)> 0)
                return;
            if (Math.Abs(e.ViewportHeightChange) + Math.Abs(e.ViewportWidthChange) > 0)
                return;
            if (e.VerticalChange < 0)
                return;
            var vo = _scroll.VerticalOffset;
            var vh = _scroll.ViewportHeight;
            var eh = _scroll.ExtentHeight;
            if (eh <= vh)//����һҳ�����ݲ��������һҳ����Ϣ
                return;
            if (vo + vh >= eh)
            {
                //_is_in_last_page = true;
                if(!_is_in_last_page)
                Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                    new Action(() => RaiseEvent(new RoutedEventArgs(LastPageEvent))));                
            }
            _is_in_last_page = vo + vh >= eh;
        }
    }
}