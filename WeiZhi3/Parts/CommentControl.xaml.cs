using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WeiZhi3.ViewModel;
using Weibo.ViewModels;

namespace WeiZhi3.Parts
{
    /// <summary>
    /// Interaction logic for CommentControl.xaml
    /// </summary>
    public partial class CommentControl : UserControl
    {
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }
        public static DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(CommentControl), null);
        public CommentControl()
        {
            InitializeComponent();
        }
        private void CommentContainerMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsExpanded = !IsExpanded;
            var cmt = (WeiboComment)DataContext;
            if (IsExpanded && cmt.replier == null)
            {
                cmt.replier = new CommentReply(cmt.status.id, cmt.id);
            }
        }
        private void OnCommentContainerKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && IsExpanded)
            {
                IsExpanded = false;
            }
            else if (e.Key == Key.Enter)
            {
                Debug.WriteLine("should submit");
                var locator = (ViewModelLocator)FindResource("Locator");
                var tb = (TextBox)sender;
                var mv = (CommentReply)tb.DataContext;
                mv.reply.Execute(locator.AccessToken);
            }
        }
    }
}
