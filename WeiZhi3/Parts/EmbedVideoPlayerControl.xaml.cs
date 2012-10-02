﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace WeiZhi3.Parts
{
    /// <summary>
    /// Interaction logic for EmbedVideoPlayer.xaml
    /// </summary>
    public partial class EmbedVideoPlayerControl : UserControl
    {
        private const string empty_content = @"<!DOCTYPE html><html scroll='no'><body scroll='no' style='color:white' bgcolor='black' ></body></html>";
        private WebBrowser _wb;
        public static DependencyProperty UrlProperty = DependencyProperty.Register("Url", typeof (string), typeof (EmbedVideoPlayerControl), new PropertyMetadata(OnUrlChanged));

        private static void OnUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EmbedVideoPlayerControl)d).OnUrlChangedImp(e);            
        }
        void OnUrlChangedImp(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null && _wb == null)
                return;
            Uri result;
            Uri.TryCreate((string)e.NewValue, UriKind.Absolute, out result);
            if (result == null && _wb == null)
                return;
            if(result == null)
            {
                _wb.NavigateToString(empty_content);
                return;
            }
            if(_wb == null)
            {
                _wb = new WebBrowser(){Height= 330};
                _wb.Navigating += OnNavigating;
                LayoutRoot.Children.Clear();
                LayoutRoot.Children.Add(_wb);
            }
            _wb.Navigate(result.AbsoluteUri);
        }

        void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            HideScriptErrors((WebBrowser) sender, true);
        }
        public EmbedVideoPlayerControl()
        {
            InitializeComponent();
            Loaded += EmbedVideoPlayerControl_Loaded;
            Unloaded += EmbedVideoPlayer_Unloaded;
        }

        void EmbedVideoPlayerControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= EmbedVideoPlayerControl_Loaded;
            var wnd = Window.GetWindow(this);
            if (wnd == null)
                return;
            wnd.CommandBindings.Add(new CommandBinding(WeiZhiCommands.PlayVideo, ExecuteWeiZhiCommandsPlayVideo));
        }

        private void ExecuteWeiZhiCommandsPlayVideo(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var url = (string) e.Parameter;
            Url = url;
        }

        void EmbedVideoPlayer_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= EmbedVideoPlayer_Unloaded;
            if(_wb != null)
            {
                _wb.Navigating -= OnNavigating;
                _wb = null;
            }
            var wnd = Window.GetWindow(this);
            if (wnd == null)
                return;
            wnd.CommandBindings.Remove(new CommandBinding(WeiZhiCommands.PlayVideo, ExecuteWeiZhiCommandsPlayVideo));
        }

        public string Url
        {
            get { return (string) GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }
        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null) return;
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }
    }
}