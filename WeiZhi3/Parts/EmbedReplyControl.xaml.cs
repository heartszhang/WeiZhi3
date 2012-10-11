using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WeiZhi3.ViewModel;
using Weibo.ViewModels;

namespace WeiZhi3.Parts
{
	/// <summary>
	/// Interaction logic for EmbedEditControl.xaml
	/// </summary>
	public partial class EmbedReplyControl : UserControl
	{
		public EmbedReplyControl()
		{
			this.InitializeComponent();
		}
        private void ExecuteWeiZhiCommandsRetweet(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var be = _body.GetBindingExpression(TextBox.TextProperty);
            Debug.Assert(be != null);
            be.UpdateSource();
            var locator = (ViewModelLocator)FindResource("Locator");
            var wr = (WeiboReply)DataContext;
            wr.retweet.Execute(locator.AccessToken);
        }

        private void ExecuteWeiZhiCommandsReply(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var be = _body.GetBindingExpression(TextBox.TextProperty);
            Debug.Assert(be != null);
            be.UpdateSource();
            var locator = (ViewModelLocator) FindResource("Locator");
            var wr = (WeiboReply) DataContext;
            wr.reply.Execute(locator.AccessToken);
        }

	    private void ExecuteWeiZhiCommandsToggleCommentFlag(object sender, ExecutedRoutedEventArgs e)
	    {
	        _reply_to_original.IsChecked = !_reply_to_original.IsChecked;
	        e.Handled = true;
	    }

	    private void OnEmbedReplyControlLoad(object sender, RoutedEventArgs e)
	    {
            _body.Focus();
	    }
	}
}