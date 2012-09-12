using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace WeiZhi3.Behaviours
{
    internal sealed class WindowChangeStateBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += ChangeStateHandler;
        }

        private void ChangeStateHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
                return;
            e.Handled = true;
            switch (AssociatedObject.WindowState)
            {
                case WindowState.Normal:
                    AssociatedObject.WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    AssociatedObject.WindowState = WindowState.Normal;
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= ChangeStateHandler;
            base.OnDetaching();
        }
    }
}