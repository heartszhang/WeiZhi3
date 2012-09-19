using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Artefact.Animation;
using WeiZhi3.ViewModel;
using Weibo.Api2.Sina;
using Weibo.ViewModels;

namespace WeiZhi3.Pages
{
	public partial class PageHome :Page
	{
		public PageHome()
		{
			this.InitializeComponent();
		    Loaded += OnPageHomeLoaded;
		}


        #region UserScreenName (Attached DependencyProperty)

        public static readonly DependencyProperty UserScreenNameProperty =
            DependencyProperty.RegisterAttached("UserScreenName", typeof(string), typeof(PageHome), new PropertyMetadata(OnUserScreenNameChanged));

        public static void SetUserScreenName(DependencyObject o, string value)
        {
            o.SetValue(UserScreenNameProperty, value);
        }

        public static string GetUserScreenName(DependencyObject o)
        {
            return (string)o.GetValue(UserScreenNameProperty);
        }

        private static void OnUserScreenNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ts = (PageHome)d;
            ts.WindowTitle = (string)e.NewValue;
        }

        #endregion
        
	    private void OnPageHomeLoaded(object sender, RoutedEventArgs e)
	    {
	        Loaded -= OnPageHomeLoaded;
	        Debug.Assert(NavigationService != null, "NavigationService != null");
	        var src = NavigationService.Source.OriginalString;
	        InitializePageHome(src);
	    }
        void InitializePageHome(string src)
        {
            var fields = src.Split(new char[] {'?','&','='}, StringSplitOptions.RemoveEmptyEntries);
            long uid = 0;
            for(var i = 0;i < fields.Length - 1;++i)
            {
                if(fields[i] == "id")
                {
                    uid = long.Parse(fields[i + 1]);
                    break;
                }
            }
            var locator = (ViewModelLocator) FindResource("Locator");
            uid= locator.Profile.Id(uid);
            var at = locator.Profile.Token(uid);
            DataContext = new PageHomeViewModel(uid,at);
        }
	    private void ToolsetPopup(object sender, System.Windows.Input.MouseEventArgs e)
		{
            _tool_container.OffsetTo(0, 0, 1, AnimationTransitions.ElasticEaseOut, 0);
		}
        private void ToolsetSlideout(object sender, System.Windows.Input.MouseEventArgs e)
		{
            _tool_container.OffsetTo(_tool_container.ActualWidth, 0, 1, AnimationTransitions.ElasticEaseOut, 0);
		}
        private void NavisetPopup(object sender, System.Windows.Input.MouseEventArgs e)
        {
			_navi_container.OffsetTo(0, 0, 1, AnimationTransitions.ElasticEaseOut, 0);
        }
        private void NavisetSlideout(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _navi_container.OffsetTo(-_navi_container.ActualWidth, 0, 1, AnimationTransitions.ElasticEaseOut, 0);
      }
	}
}