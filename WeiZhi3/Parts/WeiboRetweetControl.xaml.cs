using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
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
using WeiZhi3.ViewModel;
using Weibo.DataModel;
using Weibo.ViewModels;

namespace WeiZhi3.Parts
{
    /// <summary>
    /// Interaction logic for WeiboRetweetControl.xaml
    /// </summary>
    public partial class WeiboRetweetControl : WeiboControlBase
    {
        public WeiboRetweetControl()
        {
            InitializeComponent();
        }
    }
}
