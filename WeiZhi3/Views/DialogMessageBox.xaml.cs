using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WeiZhi3.Views
{
    public partial class DialogMessageBox : Window
    {
        public DialogMessageBox()
        {
            InitializeComponent();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.OK;
            Close();

        }
        private void OnCancelClick(object s, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.Cancel;

        }
        private void OnYesClick(object s, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.Yes;
            Close();
        }
        private void OnNoClick(object s, RoutedEventArgs e)
        {
            MessageBoxResult = MessageBoxResult.No;
        }
        public MessageBoxResult MessageBoxResult { get; set; }

        public static readonly DependencyProperty MessageProperty
            = DependencyProperty.Register("Message", typeof(string), typeof(DialogMessageBox), new UIPropertyMetadata(null));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxImageProperty
            = DependencyProperty.Register("MessageBoxImage"
            , typeof(MessageBoxImage), typeof(DialogMessageBox), new UIPropertyMetadata(MessageBoxImage.None));
        public MessageBoxImage MessageBoxImage
        {
            get
            {
                return (MessageBoxImage)GetValue(MessageBoxImageProperty);
            }
            set
            {
                SetValue(MessageBoxImageProperty, value);
            }
        }
        public static readonly DependencyProperty DefaultResultProperty
            = DependencyProperty.Register("DefaultResult"
            , typeof(MessageBoxResult)
            , typeof(DialogMessageBox)
            , new UIPropertyMetadata(MessageBoxResult.None));
        public MessageBoxResult DefaultResult
        {
            get
            {
                return (MessageBoxResult)GetValue(DefaultResultProperty);
            }
            set
            {
                SetValue(DefaultResultProperty, value);
                switch (value)
                {
                    case MessageBoxResult.None:
                    case (MessageBoxResult.Cancel | MessageBoxResult.OK):
                    case ((MessageBoxResult)4):
                    case ((MessageBoxResult)5):
                        break;

                    case MessageBoxResult.OK:
                        _ok.IsDefault = true;
                        //     _ok.Focus();
                        return;

                    case MessageBoxResult.Cancel:
                        _cancel.IsDefault = true;
                        //   _cancel.Focus();
                        return;

                    case MessageBoxResult.Yes:
                        _yes.IsDefault = true;
                        //   _yes.Focus();
                        break;

                    case MessageBoxResult.No:
                        _no.IsDefault = true;
                        //     _no.Focus();
                        return;

                    default:
                        return;
                }
            }
        }
        public static readonly DependencyProperty MessageBoxButtonProperty
            = DependencyProperty.Register("MessageBoxButton"
            , typeof(MessageBoxButton), typeof(DialogMessageBox), new UIPropertyMetadata(MessageBoxButton.OK));
        public MessageBoxButton MessageBoxButton
        {
            get
            {
                return (MessageBoxButton)GetValue(MessageBoxButtonProperty);
            }
            set
            {
                base.SetValue(MessageBoxButtonProperty, value);
                switch (value)
                {
                    case MessageBoxButton.OK:
                        this._ok.Visibility = Visibility.Visible;
                        this._ok.IsCancel = true;
                        return;

                    case MessageBoxButton.OKCancel:
                        this._ok.Visibility = Visibility.Visible;
                        this._cancel.Visibility = Visibility.Visible;
                        _cancel.IsCancel = true;
                        return;

                    case MessageBoxButton.YesNoCancel:
                        this._yes.Visibility = Visibility.Visible;
                        this._no.Visibility = Visibility.Visible;
                        this._cancel.Visibility = Visibility.Visible;
                        _cancel.IsCancel = true;
                        return;

                    case MessageBoxButton.YesNo:
                        _yes.Visibility = Visibility.Visible;
                        _no.Visibility = Visibility.Visible;
                        _no.IsCancel = true;

                        return;
                }
                _yes.Visibility = Visibility.Collapsed;
                _no.Visibility = Visibility.Collapsed;
                _cancel.Visibility = Visibility.Collapsed;
                _ok.Visibility = Visibility.Collapsed;
            }
        }

        public static MessageBoxResult Show(Window owner, string txt, string caption, MessageBoxButton button, MessageBoxResult dft)
        {
            var view = new DialogMessageBox
            {
                Title = caption,
                DefaultResult = dft,
                Message = txt,
                MessageBoxButton = button,
                MessageBoxImage = MessageBoxImage.None
            };

            if ((owner != null) && owner.IsVisible)
            {
                view.Owner = owner;
            }
            view.ShowDialog();
            return view.MessageBoxResult;
        }
        public static MessageBoxResult Show(string txt, string caption, MessageBoxButton button, MessageBoxResult dft)
        {
            return Show(Application.Current.MainWindow, txt, caption, button, dft);
        }
    }
}
