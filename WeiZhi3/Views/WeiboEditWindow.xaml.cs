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
    /// <summary>
    /// Interaction logic for WeiboEditWindow.xaml
    /// </summary>
    public partial class WeiboEditWindow : Window
    {
        public WeiboEditWindow()
        {
            InitializeComponent();
        }

        private void ExecuteCloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
