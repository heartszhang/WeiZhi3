using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace WeiZhi3.Behaviours
{
    public class ListBoxScrollToTopWhenResetBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Unloaded += OnUnLoaded;
            AssociatedObject.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
        }

        void ItemContainerGenerator_ItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Reset)
            {
                var scroll = (ScrollViewer) AssociatedObject.Template.FindName("_scroll", AssociatedObject);
                scroll.ScrollToTop();
            }
        }

        private void OnUnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Unloaded -= OnUnLoaded;

            AssociatedObject.ItemContainerGenerator.ItemsChanged -= ItemContainerGenerator_ItemsChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                int count = AssociatedObject.Items.Count;
                if (count == 0)
                    return;

                var item = AssociatedObject.Items[count - 1];

                var frameworkElement =
                    AssociatedObject.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                if (frameworkElement == null) return;

                frameworkElement.BringIntoView();
            }
        }
    }
}