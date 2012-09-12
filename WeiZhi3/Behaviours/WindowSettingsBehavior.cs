using System.Windows;
using System.Windows.Interactivity;

namespace WeiZhi3.Behaviours
{
    internal sealed class WindowSettingsBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closing += OnWindowClosed;
            LoadSettings();
        }

        private void OnWindowClosed(object sender, System.EventArgs e)
        {
            AssociatedObject.Closing -= OnWindowClosed;
            SaveSettings();
        }
        protected override void OnDetaching()
        {
            //code unreached here
            //AssociatedObject.Closed -= OnWindowClosing;
            //SaveSettings();
            base.OnDetaching();
        }
        void LoadSettings()
        {
            var props = Properties.Settings.Default;
            var state = props.WindowState;
            var l = props.WindowLeft;
            var t = props.WindowTop;
            var w = props.WindowWidth;
            var h = props.WindowHeight;
            var maxw = SystemParameters.WorkArea.Width;
            var maxh = SystemParameters.WorkArea.Height;
            if (w < double.Epsilon || double.IsInfinity(w))
                w = maxw*2/3;
            if (h < double.Epsilon || double.IsInfinity(h))
                h = maxh;
            if ((state == 0 && l < double.Epsilon) || double.IsInfinity(l))
                l = (maxw - w)/2;
            if ((state == 0 && t < double.Epsilon) || double.IsInfinity(t))
                t = (maxh - h)/2;

            AssociatedObject.WindowState = (WindowState)state;
            AssociatedObject.Width = w;
            AssociatedObject.Height = h;
            AssociatedObject.Top = t;
            AssociatedObject.Left = l;
        }
        void SaveSettings()
        {
            var props = Properties.Settings.Default;
            props.WindowState = (int)AssociatedObject.WindowState;
            
            props.WindowLeft = AssociatedObject.RestoreBounds.Left;
            props.WindowWidth = AssociatedObject.RestoreBounds.Width;
            props.WindowHeight = AssociatedObject.RestoreBounds.Height;
            props.WindowTop = AssociatedObject.RestoreBounds.Top;
            
            props.Save();
        }
    }
}