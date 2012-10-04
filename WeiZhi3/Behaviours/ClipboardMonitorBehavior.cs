using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WeiZhi3.Native;

namespace WeiZhi3.Behaviours
{
    /// <summary>
    /// </summary>
    class ClipboardMonitorBehavior : Behavior<Window>
    {
        #region

        #region ImageLocalFilePath (Attached DependencyProperty)

        public static readonly DependencyProperty ImageLocalFilePathProperty =
            DependencyProperty.RegisterAttached("ImageLocalFilePath", typeof(string), typeof(ClipboardMonitorBehavior), new PropertyMetadata(new PropertyChangedCallback(OnImageLocalFilePathChanged)));

        public static void SetImageLocalFilePath(DependencyObject o, string value)
        {
            o.SetValue(ImageLocalFilePathProperty, value);
        }

        public static string GetImageLocalFilePath(DependencyObject o)
        {
            return (string)o.GetValue(ImageLocalFilePathProperty);
        }

        private static void OnImageLocalFilePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        #endregion

        #endregion
        private bool _is_viewing;
        private IntPtr _next_viewer;
        private int _seed;
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObjectLoaded;
            AssociatedObject.Closing += AssociatedObjectClosing;

        }

        void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= AssociatedObjectLoaded;

            var helper = new WindowInteropHelper(AssociatedObject);
            var hs = HwndSource.FromHwnd(helper.Handle);
            Debug.Assert(hs != null, "hs != null");
            hs.AddHook(WinProc);
            _next_viewer = Win32.SetClipboardViewer(helper.Handle);
            _is_viewing = true;
        }

        void AssociatedObjectClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            AssociatedObject.Closing -= AssociatedObjectClosing;
            CloseClipboardMonitor();
        }
        protected override void OnDetaching()
        {
            CloseClipboardMonitor();
            base.OnDetaching();
        }
        bool LoadFromClipboardImage()
        {
            if (!Clipboard.ContainsImage())
                return false;
            var imgc = Clipboard.GetImage();
            if (imgc == null)
                return false;

            var temp = System.IO.Path.GetTempPath();
            var filepath = System.IO.Path.Combine(temp, "iweizhi_ed" + (++_seed) + ".jpg");
            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                var je = new JpegBitmapEncoder();
                je.Frames.Add(BitmapFrame.Create(imgc));
                je.Save(stream);
            }
            AssociatedObject.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                                                    (Action)(() => SetImageLocalFilePath(AssociatedObject, filepath)));
            return true;

        }
        async Task<bool> LoadFromClipboardLocalFile()
        {
            if (!Clipboard.ContainsFileDropList())
                return false;
            var fdl = Clipboard.GetFileDropList();
            return await Task.Run(() =>
            {
                foreach (var file in fdl)
                {
                    var ext = System.IO.Path.GetExtension(file);
                    if (string.IsNullOrEmpty(ext))
                        continue;
                    if (!ext.Equals(".jpeg", StringComparison.InvariantCultureIgnoreCase) &&
                        !ext.Equals(".jpg", StringComparison.InvariantCultureIgnoreCase) &&
                        !ext.Equals(".png", StringComparison.InvariantCultureIgnoreCase) &&
                        !ext.Equals(".gif", StringComparison.InvariantCultureIgnoreCase) &&
                        !ext.Equals(".bmp", StringComparison.InvariantCultureIgnoreCase)) continue;

                    Debug.WriteLine(file);

                    var tmp = System.IO.Path.GetTempPath();
                    var filepath = System.IO.Path.Combine(tmp, "iweizhi-ed" + (++_seed) + System.IO.Path.GetExtension(file));
                    File.Copy(file, filepath, true);
                    AssociatedObject.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                                                            (Action)(() => SetImageLocalFilePath(AssociatedObject, filepath)));
                    return true;
                }
                return false;

            });
        }
        async Task<bool> LoadFromClipboardLocalFilePath()
        {
            //(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.]+)\\(?:[\w]+\\)*\w([\w.])+
            if (!Clipboard.ContainsText(TextDataFormat.Text))
                return false;
            var txt = Clipboard.GetText(TextDataFormat.Text);
            return await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(txt))
                    return false;
                var m = Regex.Match(txt, @"(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.]+)\\(?:[\w]+\\)*\w([\w.])+");
                if (!m.Success)
                    return false;
                var file = m.Groups[0].Value;
                Debug.WriteLine(file);
                if (!File.Exists(file))
                    return false;
                var tmp = System.IO.Path.GetTempPath();
                var filepath = System.IO.Path.Combine(tmp, "iweizhi-ed" + (++_seed) + System.IO.Path.GetExtension(file));
                File.Copy(file, filepath, true);
                AssociatedObject.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                                                        (Action)(() => SetImageLocalFilePath(AssociatedObject, filepath)));
                return true;
            });
        }
        static string ExtentionFromContentType(string mediatype)
        {
            var fields = mediatype.ToLower().Split(new char[] { '/' });
            if (fields.Length != 2)
                return string.Empty;
            var rtn = "." + fields[1];
            switch (fields[1])
            {
                //            X-Windows bitmap (b/w)  xbm    image/x-xbitmap 
            case "x-xbitmap":
                rtn = ".xbm";
                break;
                //            X-Windows pixelmap (8-bit color)  xpm    image/x-xpixmap 
            case "x-xpixmap":
                rtn = ".xpm";
                break;
                //Portable Network Graphics png    image/x-png 
            case "x-png":
                rtn = ".png";
                break;
                //            Group III Fax (RFC 1494) g3f    image/g3fax 
            case "g3fax":
                rtn = ".g3f";
                break;
            }
            return rtn;
            /*
            GIF  gif   image/gif 
            Image Exchange Format (RFC 1314) ief    image/ief 
            JPEG  jpeg jpg jpe   image/jpeg 
            TIFF  tiff tif   image/tiff 
            RGB  rgb   image/rgb 
                  image/x-rgb  
            X Windowdump format xwd   image/x-xwindowdump 
            Macintosh PICT format pict    image/x-pict 
            PPM (UNIX PPM package) ppm    image/x-portable-pixmap 
            PGM (UNIX PPM package) pgm    image/x-portable-graymap  
            PBM (UNIX PPM package) pbm    image/x-portable-bitmap 
            PNM (UNIX PPM package) pnm    image/x-portable-anymap 
            Microsoft Windows bitmap  bmp    image/x-ms-bmp 
            CMU raster  ras   image/x-cmu-raster 
            Kodak Photo-CD  pcd   image/x-photo-cd 
            Computer Graphics Metafile  cgm    image/cgm 
            North Am. Presentation Layer Protocol     image/naplps 
            CALS Type 1 or 2 mil cal    image/x-cals 
            Fractal Image Format (Iterated Systems)  fif   image/fif  
            QuickSilver active image (Micrografx)  dsf   image/x-mgx-dsf  
            CMX vector image (Corel) cmx    image/x-cmx 
            Wavelet-compressed (Summus) wi    image/wavelet 
            AutoCad Drawing (SoftSource) dwg    image/vnd.dwg 
                  image/x-dwg  
            AutoCad DXF file (SoftSource) dxf    image/vnd.dxf 
                  image/x-dxf  
            Simple Vector Format (SoftSource) svf   image/vnd.svf  
                  also vector/x-svf  
             */
        }
        async Task<bool> LoadFromClipboardHyperlink()
        {
            if (!Clipboard.ContainsText(TextDataFormat.Text))
                return false;
            var txt = Clipboard.GetText(TextDataFormat.Text);
            if (string.IsNullOrEmpty(txt))
                return false;
            var m = Regex.Match(txt, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase);
            if (!m.Success)
                return false;
            var url = m.Groups[0].Value;
            Debug.WriteLine(url);
            using (var client = new HttpClient())
            {
                var resp = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                if (!resp.IsSuccessStatusCode)
                    return false;
                if (!resp.Content.Headers.ContentType.MediaType.Contains("image"))
                    return false;

                var tmp = System.IO.Path.GetTempPath();
                var ext = ExtentionFromContentType(resp.Content.Headers.ContentType.MediaType);
                var filepath = System.IO.Path.Combine(tmp, "iweizhi-ed" + (++_seed) + ext);
                using (var stream = File.Create(filepath))
                {
                    await resp.Content.CopyToAsync(stream);
                }
                await AssociatedObject.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                                                              (Action)(() => SetImageLocalFilePath(AssociatedObject, filepath)));
                return true;
            }
        }

        async void DrawContent()
        {
            var r = LoadFromClipboardImage();
            if (!r)
                r = await LoadFromClipboardLocalFile();
            if (!r)
                r = await LoadFromClipboardLocalFilePath();
            if (!r)
                r = await LoadFromClipboardHyperlink();
        }
        public void CloseClipboardMonitor()
        {
            if (_is_viewing == false)
                return;
            var helper = new WindowInteropHelper(AssociatedObject);
            var hs = HwndSource.FromHwnd(helper.Handle);

            Win32.ChangeClipboardChain(helper.Handle, _next_viewer);
            _next_viewer = IntPtr.Zero;
            Debug.Assert(hs != null, "hs != null");
            hs.RemoveHook(WinProc);
            _is_viewing = false;
        }
        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
            case Win32.WM_CHANGECBCHAIN:
                if (wParam == _next_viewer)
                {
                    // clipboard viewer chain changed, need to fix it.
                    _next_viewer = lParam;
                }
                else if (_next_viewer != IntPtr.Zero)
                {
                    // pass the message to the next viewer.
                    Win32.SendMessage(_next_viewer, msg, wParam, lParam);
                }
                break;

            case Win32.WM_DRAWCLIPBOARD:
                // clipboard content changed
                DrawContent();
                // pass the message to the next viewer.
                Win32.SendMessage(_next_viewer, msg, wParam, lParam);
                break;
            }

            return IntPtr.Zero;
        }

    }
}