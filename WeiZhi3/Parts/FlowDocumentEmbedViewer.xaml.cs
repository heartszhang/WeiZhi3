using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Threading;
using NReadability;
using Ude;
using Weibo.Apis.Net;

namespace WeiZhi3.Parts
{
    /// <summary>
    /// Interaction logic for FlowDocumentEmbedViewer.xaml
    /// </summary>
    public partial class FlowDocumentEmbedViewer : UserControl
    {
        public FlowDocumentEmbedViewer()
        {
            InitializeComponent();
        }


        public string ReasonPhrase
        {
            get { return (string)GetValue(ReasonPhraseProperty); }
            set { SetValue(ReasonPhraseProperty, value); }
        }
        public static DependencyProperty ReasonPhraseProperty =
            DependencyProperty.Register("ReasonPhrase", typeof(string), typeof(FlowDocumentEmbedViewer),new PropertyMetadata("Loading..."));

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }
        public static DependencyProperty UrlProperty = DependencyProperty.Register("Url", typeof(string), typeof(FlowDocumentEmbedViewer),
            new PropertyMetadata(OnUrlChanged));

        private static void OnUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = (FlowDocumentEmbedViewer)d;
            if (DesignerProperties.GetIsInDesignMode(self))
                return;

            Task.Run(() => self.OnUrlChangedImp(e));
        }
        void UiInvoke(Action act)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, act);
        }
        private async void OnUrlChangedImp(DependencyPropertyChangedEventArgs e)
        {
            var u = (string)e.NewValue;
            if (string.IsNullOrEmpty(u))
                return;
            UiInvoke(() => ReasonPhrase = u);
            var fp = await HttpDownloadToLocalFile.DownloadAsync(u, "html", ".htm","text/html", 1*1024* 1024);
            if(string.IsNullOrEmpty(fp) || !File.Exists(fp))
            {
                UiInvoke(()=>ReasonPhrase = "Download file failed");
                return;
            }
            var fdocn = fp + ".xaml";
            if(!File.Exists(fdocn))
            {
                var charset = File.ReadAllText(fp + ".enc");
                if(string.IsNullOrEmpty(charset))
                {
                    charset = DetectEncoding(fp);
                }
                var enc = string.IsNullOrEmpty(charset) ? Encoding.GetEncoding(936) : Encoding.GetEncoding(charset);

                UiInvoke(()=>ReasonPhrase = "Converting...");
                var result = new NReadabilityTranscoder().Transcode(new TranscodingInput(File.ReadAllText(fp,enc)) { Url = u });
                if(result.ContentExtracted)
                {
                    File.WriteAllText(fdocn,result.ExtractedContent);
                    UiInvoke(()=>ReasonPhrase = "Flow document ready");
                }else
                {
                    UiInvoke(()=>ReasonPhrase = "Converte failed");
                    return;
                }

            }
            UiInvoke(() =>
            {
                var fdoc = (FlowDocument)XamlReader.Load(File.OpenRead(fdocn));
                if (fdoc == null)
                {
                    ReasonPhrase = "convert html to readability flow document failed";
                    return;
                }
                _container.Children.Clear();
                var fv = new FlowDocumentScrollViewer { Document = fdoc };
                _container.Children.Add(fv);
            });
           
        }
        string DetectEncoding(string fp)
        {
            using (var fs = File.OpenRead(fp))
            {
                ICharsetDetector cdet = new CharsetDetector();
                cdet.Feed(fs);
                cdet.DataEnd();
                return cdet.Charset;
            }
        }                               
    }
}
