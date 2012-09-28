using System;
using System.ComponentModel;
using System.Diagnostics;
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

        #region ThumbnailUrl (Attached DependencyProperty)

        public static readonly DependencyProperty ThumbnailUrlProperty =
            DependencyProperty.RegisterAttached("ThumbnailUrl", typeof(string), typeof(ImageWeiboLocalCache));

        public static void SetThumbnailUrl(DependencyObject o, string value)
        {
            o.SetValue(ThumbnailUrlProperty, value);
        }

        public static string GetThumbnailUrl(DependencyObject o)
        {
            return (string)o.GetValue(ThumbnailUrlProperty);
        }

        #endregion

        #region MiddlePicUrl (Attached DependencyProperty)

        public static readonly DependencyProperty MiddlePicUrlProperty =
            DependencyProperty.RegisterAttached("MiddlePicUrl", typeof(string), typeof(ImageWeiboLocalCache));

        public static void SetMiddlePicUrl(DependencyObject o, string value)
        {
            o.SetValue(MiddlePicUrlProperty, value);
        }

        public static string GetMiddlePicUrl(DependencyObject o)
        {
            return (string)o.GetValue(MiddlePicUrlProperty);
        }

        #endregion

        #region ImageMode (Attached DependencyProperty)

        public static readonly DependencyProperty ImageModeProperty =
            DependencyProperty.RegisterAttached("ImageMode", typeof(int), typeof(ImageWeiboLocalCache));

        public static void SetImageMode(DependencyObject o, int value)
        {
            o.SetValue(ImageModeProperty, value);
        }

        public static int GetImageMode(DependencyObject o)
        {
            return (int)o.GetValue(ImageModeProperty);
        }

        #endregion

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

        static async void ImgMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var img = (Image) sender;
            if (img == null)
                return;
           // e.Handled = true;//需要使用这个函数设置selecteditem
            var mode = (int) img.GetValue(ImageModeProperty);
            if (mode == 0 || mode == 1)//unintialized,or intermediate
                return;

            var middlepic = (string) img.GetValue(MiddlePicUrlProperty);
            var thumbnailpic = (string) img.GetValue(ThumbnailUrlProperty);
            string url;
            if(mode == 2)//thumbnail mode
            {
                //img.SetValue(ImageModeProperty, 3);
                mode = 3;
                url = await HttpDownloadToLocalFile.DownloadAsync(middlepic, "bmiddle", ".jpg");
            }
            else //middle pic mode
            {
                //img.SetValue(ImageModeProperty, 2);
                mode = 2;
                url = await HttpDownloadToLocalFile.DownloadAsync(thumbnailpic, "thumbnail", ".jpg");
            }
            if (string.IsNullOrEmpty(url))
                return;
            SetImageSource(img, url, mode);
        }

        static void UiInvoke(DispatcherObject img, Action act)
        {
            img.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle,act);
        }
        public static void SetImageSource(Image img, string url, int mode)
        {
            if (string.IsNullOrEmpty(url))
                return;
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(url, UriKind.Absolute);
            bmp.EndInit();
            bmp.Freeze();
            var bs = (bmp.DpiX > 160.0) ? ConvertBitmapTo96Dpi(bmp) : bmp;
            try
            {
                img.Dispatcher.BeginInvoke(
                    DispatcherPriority.SystemIdle,
                    (Action) (() =>
                    {
                        if (img.Source == null || !img.Source.Equals(bs))
                        {
                            img.Source = bs;
                            img.SetValue(ImageModeProperty,mode);
                        }
                    }));
            }catch(System.NotSupportedException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        private static async void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
                return;
            var i = (WeiboStatus) e.NewValue;
            var img = (Image)d;
            if (img == null || i == null || !i.has_pic)
                return;
            img.SetValue(ThumbnailUrlProperty, i.thumbnail_pic);
            img.SetValue(MiddlePicUrlProperty, i.bmiddle_pic);

            var mode = (int) img.GetValue(ImageModeProperty);
            if(mode == 0)
            {
                img.SetValue(ImageModeProperty, 1);
                img.MouseLeftButtonDown += ImgMouseLeftButtonDown;
            }

            var sz = await Utils.FetchImageSizeAsync(i.thumbnail_pic);
            if (sz.Height * sz.Width == 0)
                return;
            

            string url;
            if(sz.Height > sz.Width)
            {//portrait
                //UiInvoke(img, ()=>img.SetValue(ImageModeProperty, 2));
                mode = 2;
                url = await HttpDownloadToLocalFile.DownloadAsync(i.thumbnail_pic, "thumbnail", ".jpg");
            }else
            {
                //UiInvoke(img, () => img.SetValue(ImageModeProperty, 3));
                mode = 3;
                url = await HttpDownloadToLocalFile.DownloadAsync(i.bmiddle_pic, "bmiddle", ".jpg");
            }
            if (string.IsNullOrEmpty(url))
                url = i.thumbnail_pic;
            SetImageSource(img, url, mode);
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