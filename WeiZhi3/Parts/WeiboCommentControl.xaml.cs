using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Artefact.Animation;
using WeiZhi3.ViewModel;
using Weibo.ViewModels;

namespace WeiZhi3.Parts
{
	/// <summary>
	/// Interaction logic for CommentReplyControl.xaml
	/// </summary>
	public partial class WeiboCommentControl : UserControl
	{
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }
        public static DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(WeiboCommentControl), null);
        
		public WeiboCommentControl()
		{
			this.InitializeComponent();
		}

        private void CommentContainerMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {            
            IsExpanded = !IsExpanded;
            var cmt = (WeiboComment) DataContext;
            if(IsExpanded && cmt.replier == null)
            {
                cmt.replier = new CommentReply(cmt.status.id, cmt.id);
            }
        }
	    private void OnCommentContainerKeyDown(object sender, KeyEventArgs e)
	    {
            if (e.Key == Key.Escape && IsExpanded)
            {
                IsExpanded = false;
            }if(e.Key == Key.Enter)
	        {
	            Debug.WriteLine("should submit");
	            var locator = (ViewModelLocator) FindResource("Locator");
                WeiZhiCommands.ReplyComment.Execute(locator.AccessToken,this);
	        }
	    }
	}
}