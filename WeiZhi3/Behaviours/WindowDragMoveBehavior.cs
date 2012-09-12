using System.Windows;
using System.Windows.Interactivity;

namespace WeiZhi3.Behaviours
{
    internal sealed class WindowDragMoveBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += DragMoveHandler;
        }
        private void DragMoveHandler(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (AssociatedObject.WindowState != WindowState.Normal)
                return;
            AssociatedObject.DragMove();
        }
        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= DragMoveHandler;
            base.OnDetaching();
        }
    }
}