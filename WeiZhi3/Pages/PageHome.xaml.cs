using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using Artefact.Animation;
using WeiZhi3.ViewModel;
using Weibo.ViewModels;
using System.Runtime.Caching;

namespace WeiZhi3.Pages
{

	public partial class PageHome :Page
	{
	    private long _user_id;
	    private Timer _timer;
		public PageHome()
		{
			this.InitializeComponent();
		    Loaded += OnPageHomeLoaded;
            Unloaded += OnPageHomeUnloaded;
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


        void OnPageHomeUnloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= OnPageHomeUnloaded;

            if(_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
            //do some cleanup here
        }

        private void OnPageHomeLoaded(object sender, RoutedEventArgs e)
	    {
	        Loaded -= OnPageHomeLoaded;
	        Debug.Assert(NavigationService != null, "NavigationService != null");
	        var src = NavigationService.Source.OriginalString;
	        InitializePageHome(src);

            var settings = Properties.Settings.Default;
            _timer = new Timer(OnTimerCallback, DataContext, settings.TimelineTickInterval, settings.TimelineTickInterval);
	    }

	    private void OnTimerCallback(object state)
	    {
            var vm = (PageHomeViewModel)state;
            Debug.Assert(vm != null);

            vm.OnTick(Token());
        }

        string Token()
        {
            var locator = (ViewModelLocator)FindResource("Locator");
            return locator.Profile.Token(_user_id);            
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
            _user_id = uid = locator.Profile.Id(uid);

	        var at = locator.Profile.Token(uid);
	        CacheToken(at);
            var vm = new PageHomeViewModel(uid,at);
            DataContext = vm;
            vm.Initialize(at);
        }
        void CacheToken(string token)
        {
            var mem = MemoryCache.Default;
            mem.Set("current_token",token,DateTimeOffset.Now.AddDays(1.0));
        }
	    private void ToolsetPopup(object sender, System.Windows.Input.MouseEventArgs e)
		{
            _tool_container.OffsetTo(0, 0, 1, AnimationTransitions.ExpoEaseOut, 0);
		}
        private void ToolsetSlideout(object sender, System.Windows.Input.MouseEventArgs e)
		{
            _tool_container.OffsetTo(_tool_container.ActualWidth, 0, 1, AnimationTransitions.ExpoEaseOut, 0);
		}
        private void NavisetPopup(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _navi_container.OffsetTo(0, 0, 1, AnimationTransitions.ExpoEaseOut, 0);
        }
        private void NavisetSlideout(object sender, System.Windows.Input.MouseEventArgs e)
        {
            _navi_container.OffsetTo(-_navi_container.ActualWidth, 0, 1, AnimationTransitions.ExpoEaseOut, 0);
        }
        TimelineViewModel GetTimeline()
        {
            var vm = (PageHomeViewModel) DataContext;
            return vm.Timeline;
        }

        private void OnNextPageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var tvm = GetTimeline();
            tvm.NextPage(Token());
        }

        private void OnPreviousPageExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            GetTimeline().PreviousPage(Token());
        }

        private void CanCommandExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
    }
}