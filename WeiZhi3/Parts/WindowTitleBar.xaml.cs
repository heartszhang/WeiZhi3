using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeiZhi3.Parts
{
    /// <summary>
    /// Interaction logic for WindowTitleBar.xaml
    /// </summary>
    public partial class WindowTitleBar
    {
        public static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof (string), typeof (WindowTitleBar), new PropertyMetadata(default(string)));

        public WindowTitleBar()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private void ButtonCloseClick(object sender, RoutedEventArgs e)
        {
            var wnd = Window.GetWindow(this);
            if(wnd != null)
                wnd.Close();
        }
    }
}
