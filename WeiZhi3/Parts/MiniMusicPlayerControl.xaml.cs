using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Weibo.DataModel;

namespace WeiZhi3.Parts
{
    /// <summary>
    /// Interaction logic for MiniMusicPlayer.xaml
    /// </summary>
    public partial class MiniMusicPlayerControl : UserControl
    {
        private DispatcherTimer _tick;
        public MiniMusicPlayerControl()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            _tick = new DispatcherTimer(TimeSpan.FromSeconds(0.3), DispatcherPriority.SystemIdle, OnTick, Dispatcher);
            Loaded += MiniMusicPlayerControlLoaded;
            Unloaded -= MiniMusicPlayerControlUnloaded;
        }
        void MiniMusicPlayerControlUnloaded(object s, RoutedEventArgs e)
        {
            Unloaded -= MiniMusicPlayerControlUnloaded;
            var wnd = Window.GetWindow(this);
            if (wnd == null)
                return;
            wnd.CommandBindings.Remove(new CommandBinding(WeiZhiCommands.PlayMusic, ExecuteMediaCommandsPlay));
            //wnd.CommandBindings.Remove(new CommandBinding(MediaCommands.TogglePlayPause,
            //                                           ExecuteMediaCommandsTogglePlayPause));
            //wnd.CommandBindings.Remove(new CommandBinding(MediaCommands.Pause, ExecuteMediaCommandsPause));

        }
        void MiniMusicPlayerControlLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= MiniMusicPlayerControlLoaded;
            var wnd = Window.GetWindow(this);
            Debug.Assert(wnd != null);
            wnd.CommandBindings.Add(new CommandBinding(WeiZhiCommands.PlayMusic, ExecuteMediaCommandsPlay));
            //wnd.CommandBindings.Add(new CommandBinding(MediaCommands.TogglePlayPause,
            //                                           ExecuteMediaCommandsTogglePlayPause));
            //wnd.CommandBindings.Add(new CommandBinding(MediaCommands.Pause, ExecuteMediaCommandsPause));
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (!IsPlaying)
                return;
            var pos = _player.Position;
            var du = _player.NaturalDuration;
            if(!du.HasTimeSpan)
            {
                Progress = 0.0;
            }else
            {
                Progress = pos.TotalMilliseconds * 100/(du.TimeSpan.TotalMilliseconds + 1);
            }
            DownloadingProgress = _player.DownloadProgress;
        }

        public string Prompt
        {
            get { return (string)GetValue(PromptProperty); }
            set { SetValue(PromptProperty, value); }
        }
        public static DependencyProperty PromptProperty = DependencyProperty.Register("Prompt", typeof(string), typeof(MiniMusicPlayerControl), null);
        
        internal Song Current
        {
            get { return (Song)GetValue(CurrentProperty); }
            set { SetValue(CurrentProperty, value); }
        }

        public bool IsPlaying
        {
            get { return (bool) GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        public static DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(double), typeof(MiniMusicPlayerControl), null);
        
        public double DownloadingProgress
        {
            get { return (double) GetValue(DownloadingProgressProperty); }
            set { SetValue(DownloadingProgressProperty, value); }
        }

        public static DependencyProperty CurrentProperty = DependencyProperty.Register("Current", typeof(Song), typeof(MiniMusicPlayerControl), null);
        public static DependencyProperty IsPlayingProperty = DependencyProperty.Register("IsPlaying", typeof (bool), typeof (MiniMusicPlayerControl), new PropertyMetadata(default(bool)));
        public static DependencyProperty DownloadingProgressProperty = DependencyProperty.Register("DownloadingProgress", typeof (double), typeof (MiniMusicPlayerControl), new PropertyMetadata(default(double)));

        async void Play(Song s)
        {
            Current = s;
            var mp3 = await Current.FetchMp3();
            if (string.IsNullOrEmpty(mp3))
            {
                Prompt = "Expand url faield";
                return;
            }
            Prompt = "Fetching Music...";
            await Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, (Action)(() =>
            {
                Prompt = null;
                _player.Source = new Uri(mp3);
                _player.Play();
            }));
        }
        private void ExecuteMediaCommandsPlay(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var s = new Song((UrlInfo) e.Parameter);

            Play(s);
        }
        private void ExecuteMediaCommandsPause(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = false;
            _player.Pause();
        }
        private void ExecuteMediaCommandsTogglePlayPause(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var pausing = (bool) e.Parameter;
            if (pausing)
                _player.Play();
            else _player.Pause();
        }

        private void OnMediaEnded(object sender, RoutedEventArgs e)
        {
            IsPlaying = false;
        }

        private void OnMediaOpened(object sender, RoutedEventArgs e)
        {
            Prompt = string.Empty;
            IsPlaying = true;
        }

        private void OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Prompt = "Failed";
            IsPlaying = false;
        }
    }
}
