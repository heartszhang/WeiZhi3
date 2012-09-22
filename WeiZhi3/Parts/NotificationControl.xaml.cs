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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;

namespace WeiZhi3.Parts
{
    /// <summary>
    /// Interaction logic for NotificationControl.xaml
    /// </summary>
    public partial class NotificationControl : UserControl
    {
        public static DependencyProperty NotificationTextProperty = DependencyProperty.Register("NotificationText", typeof (string), typeof (NotificationControl), new PropertyMetadata(default(string)));

        public NotificationControl()
        {
            InitializeComponent();
            Loaded += NotificationControl_Loaded;
            Unloaded += NotificationControl_Unloaded;
        }

        void NotificationControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= NotificationControl_Unloaded;
            Messenger.Default.Unregister(this);
        }

        void NotificationControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= NotificationControl_Loaded;
            Messenger.Default.Register<NotificationMessage>(this, "noti" ,OnNotificationReceived);
        }
        void UiBeginInvoke(Action act)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, act);
        }
        private void OnNotificationReceived(NotificationMessage obj)
        {
            UiBeginInvoke(()=>
                {
                    NotificationText = obj.Notification;
                });
        }

        public string NotificationText
        {
            get { return (string) GetValue(NotificationTextProperty); }
            set { SetValue(NotificationTextProperty, value); }
        }
    }
}
