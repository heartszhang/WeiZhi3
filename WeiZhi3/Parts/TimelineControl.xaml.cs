using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Weibo.ViewModels;

namespace WeiZhi3.Parts
{
    /// <summary>
    /// Interaction logic for PartMainContent.xaml
    /// </summary>
    public partial class TimelineControl : UserControl
    {
        public TimelineControl()
        {
            InitializeComponent();
            
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            if (e.AddedItems.Count != 1)
                return;
            var item = (WeiboStatus)e.AddedItems[0];
            var vm = (TimelineViewModel) DataContext;
            if (item.has_pic)
                vm.FocusedItem = item.bmiddle_pic;
        }
    }
}
