using System.Windows.Media.Imaging;

namespace WeiZhi3.Attached
{
    internal class ImageLocalCacheBase
    {
        internal static BitmapSource ConvertBitmapTo96Dpi(BitmapImage bitmap_image)
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