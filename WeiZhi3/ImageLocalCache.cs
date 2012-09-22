using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Weibo.Apis.Net;

namespace WeiZhi3
{
    internal class ImageLocalCache
    {
        private static async void DownloadImageFile(string url, Image img)
        {
            if (string.IsNullOrEmpty(url))
                return;
            var uri = new Uri(url, UriKind.Absolute);

            if (!uri.IsFile && !uri.IsUnc)
            {
                var fpc = await HttpDownloadToLocalFile.DownloadAsync(url, "middle", ".jpg");
                if (!File.Exists(fpc))
                    return;
                uri = new Uri(fpc, UriKind.Absolute);
            }

            var bmp = new BitmapImage();
            bmp.BeginInit();

            bmp.UriSource = uri;
            bmp.EndInit();
            bmp.Freeze();
            var bs = (bmp.DpiX > 160.0) ? ConvertBitmapTo96Dpi(bmp) : bmp;
            await img.Dispatcher.BeginInvoke(
                DispatcherPriority.SystemIdle,
                (Action) (() =>
                    {
                        if (img.Source == null || !img.Source.Equals(bs))
                            img.Source = bs;
                    }));
            
        }
        private static void OnUrlSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var img = (Image)d;
            if (img == null || DesignerProperties.GetIsInDesignMode(img))
                return;
            if (e.NewValue is ImageSource)
            {
                img.Source = e.NewValue as ImageSource;
                return;
            }
            if (e.NewValue == null)
                return;
            DownloadImageFile(e.NewValue as string, img);

        }
        public static BitmapSource ConvertBitmapTo96Dpi(BitmapImage bitmap_image)
        {
            const double dpi = 96;
            var width = bitmap_image.PixelWidth;
            var height = bitmap_image.PixelHeight;
            var stride = (bitmap_image.Format.BitsPerPixel * width + 7) / 8;
            var pixel_data = new byte[stride * height];
            bitmap_image.CopyPixels(pixel_data, stride, 0);

            var rtn = BitmapSource.Create(width, height, dpi, dpi, bitmap_image.Format, null, pixel_data, stride);
            if (rtn.CanFreeze)
                rtn.Freeze();

            return rtn;
        }
        public static readonly DependencyProperty UrlSourceProperty = DependencyProperty.RegisterAttached("UrlSource", typeof(string), typeof(ImageLocalCache), new FrameworkPropertyMetadata(OnUrlSourceChanged));

        public static string GetUrlSource(UIElement img)
        {
            return img.GetValue(UrlSourceProperty) as string;
        }
        public static void SetUrlSource(UIElement img, string val)
        {
            img.SetValue(UrlSourceProperty, val);
        }
    }
}