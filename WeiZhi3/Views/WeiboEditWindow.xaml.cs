using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WeiZhi3.Behaviours;
using Weibo.ViewModels;

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
            Owner.Activate();
            Close();
        }

        /*
        private void ExecuteWeiZhiCommandsSubmit(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var vm = (WeiboEditViewModel) DataContext;
            vm.submit.Execute(e.Parameter);
        }*/

        //private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key != Key.Enter)
        //        return;
        //    var tb = (TextBox)sender;
        //    var be = tb.GetBindingExpression(TextBox.TextProperty);
        //    Debug.Assert(be != null);
        //    be.UpdateSource();            
        //}

        private void ExecuteWeiZhiCommandsSelectImage(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".jpg",
                Filter = "Image Files (.jpg)|*.jpg;*.jpeg;*.png|All Files(.*)|*.*",
            };

            // Show open file dialog box
            var result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                //string filename = dlg.FileName;
                ClipboardMonitorBehavior.SetImageLocalFilePath(this, dlg.FileName);
            }
        }

        private void ExecuteClearImage(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ClipboardMonitorBehavior.SetImageLocalFilePath(this, null);
        }
    }
}
