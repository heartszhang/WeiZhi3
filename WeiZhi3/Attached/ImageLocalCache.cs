using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Weibo.Apis.Net;

namespace WeiZhi3.Attached
{
    internal class ImageLocalCache:ImageLocalCacheBase
    {

        #region Url (Attached DependencyProperty)

        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.RegisterAttached("Url", typeof(string), typeof(ImageLocalCache), new PropertyMetadata(new PropertyChangedCallback(OnUrlChanged)));

        public static void SetUrl(DependencyObject o, string value)
        {
            o.SetValue(UrlProperty, value);
        }

        public static string GetUrl(DependencyObject o)
        {
            return (string)o.GetValue(UrlProperty);
        }

        private static async void OnUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var img = (Image) d;
            var url = (string)e.NewValue;
            if (string.IsNullOrEmpty(url))
                return;
            var pic = await HttpDownloadToLocalFile.DownloadAsync(url, "thumbnail", ".jpg");
            if (!File.Exists(pic))
                return;
            SetImageSource(img, pic);
        }

        #endregion
        static void SetImageSource(Image img, string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
                return;
            var ur = new Uri(filepath, UriKind.Absolute);
            Debug.Assert(ur.IsFile);
            try
            {
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = ur;
                bmp.EndInit();
                bmp.Freeze();
                var bs = (bmp.DpiX > 160.0) ? ConvertBitmapTo96Dpi(bmp) : bmp;
                img.Dispatcher.BeginInvoke(
                    DispatcherPriority.SystemIdle,
                    (Action)(() =>
                    {
                        if (img.Source != null && img.Source.Equals(bs)) return;
                        img.Source = bs;
                    }));
            }
            catch (System.NotSupportedException e)//文件可能被锁住，导致出现这个异常
            {
                Debug.WriteLine(e.Message);
            }
        }

    }
}