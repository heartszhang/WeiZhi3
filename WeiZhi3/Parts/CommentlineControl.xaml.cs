using System;
using System.Collections.Generic;
using System.Linq;
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
using Artefact.Animation;
using WeiZhi3.ViewModel;
using Weibo.ViewModels;

namespace WeiZhi3.Parts
{
    /// <summary>
    /// Interaction logic for CommentlineControl.xaml
    /// </summary>
    public partial class CommentlineControl : UserControl
    {
        public CommentlineControl()
        {
            InitializeComponent();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            return;
            //e.Handled = true;
            //if (e.AddedItems.Count != 1)
            //    return;
            //var item = (WeiboStatus)e.AddedItems[0];
            //var vm = (TimelineViewModel) DataContext;
            //if (item.has_pic)
            //    vm.FocusedItem = item.bmiddle_pic;
        }

        private void OnMouseEnterItemContainer(object sender, MouseEventArgs e)
        {
            var item = (ListBoxItem)sender;
            if (item == null)
            {
                _items.HoveredItem = null;
                return;
            }
            var ws = item.DataContext;
            _items.HoveredItem = ws;
        }

        private void OnMouseEnterToolset(object sender, MouseEventArgs e)
        {
            _toolset.OffsetTo(0, 0, 0.2, AnimationTransitions.ExpoEaseOut, 0);
        }

        private void OnMouseLeaveToolset(object sender, MouseEventArgs e)
        {
            _toolset.OffsetTo(0, _toolset.ActualHeight, 0.2, AnimationTransitions.ExpoEaseOut, 0);
        }

        private void ExecuteWeiZhiCommandsScrollDown(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var alpha = Properties.Settings.Default.ScrollAlpha;
            _items.Scroll(alpha);
        }
        private void ExecuteWeiZhiCommandsScrollUp(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var alpha = Properties.Settings.Default.ScrollAlpha;
            _items.Scroll(-alpha);
        }

        private void OnLastPage(object sender, RoutedEventArgs e)
        {
            var at = ((ViewModelLocator)FindResource("Locator")).AccessToken;
            var vm = (CommentlineViewModel) DataContext;
            vm.MorePage(at.get());
            e.Handled = true;
        }
    }
}
