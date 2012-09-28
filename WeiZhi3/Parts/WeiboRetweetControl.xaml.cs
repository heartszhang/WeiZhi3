﻿using System;
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
using Weibo.ViewModels;

namespace WeiZhi3.Parts
{
    /// <summary>
    /// Interaction logic for WeiboRetweetControl.xaml
    /// </summary>
    public partial class WeiboRetweetControl : UserControl
    {
        public WeiboRetweetControl()
        {
            InitializeComponent();
        }
        private void OnMouseLeftButtonDownImp(object sender, MouseButtonEventArgs e)
        {
            var vm = (WeiboStatus)DataContext;
            vm.show_editor.Execute(null);
        }
    }
}