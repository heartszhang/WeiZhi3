using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Weibo.Apis;
using Weibo.Apis.Net;
using Weibo.ViewModels;

namespace WeiZhi3.Attached
{
    internal class ImageWeiboLocalCache
    {

        #region Item (Attached DependencyProperty)

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.RegisterAttached("Item", typeof(WeiboStatus), typeof(ImageWeiboLocalCache), new PropertyMetadata(new PropertyChangedCallback(OnItemChanged)));

        public static void SetItem(DependencyObject o, WeiboStatus value)
        {
            o.SetValue(ItemProperty, value);
        }

        public static WeiboStatus GetItem(DependencyObject o)
        {
            return (WeiboStatus)o.GetValue(ItemProperty);
        }

        private static async void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var i = (WeiboStatus) e.NewValue;
            var img = (Image)d;
            if (img == null || i == null || !i.has_pic)
                return;
            var sz = await Utils.FetchImageSizeAsync(i.thumbnail_pic);
            if (sz.Height * sz.Width == 0)
                return;
            string url;
            if(sz.Height > sz.Width)
            {//portrait
                url = await HttpDownloadToLocalFile.DownloadAsync(i.thumbnail_pic, "thumbnail", ".jpg");
            }else
            {
                url = await HttpDownloadToLocalFile.DownloadAsync(i.bmiddle_pic, "bmiddle", ".jpg");
            }
            if (string.IsNullOrEmpty(url))
                url = i.thumbnail_pic;

            var bmp = new BitmapImage();
            bmp.BeginInit();

            bmp.UriSource = new Uri(url,UriKind.Absolute);
            bmp.EndInit();
            bmp.Freeze();
            var bs = (bmp.DpiX > 160.0) ? ConvertBitmapTo96Dpi(bmp) : bmp;
            img.Dispatcher.Invoke(
                DispatcherPriority.SystemIdle,
                (Action) (() =>
                    {
                        if (img.Source == null || !img.Source.Equals(bs))
                            img.Source = bs;
                    }));
        }

        #endregion
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
       
    }
}