using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System.Windows.Media;
namespace WeiZhi3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {        
        static App()
        {
            DispatcherHelper.Initialize();
        }
        public App()
        {
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Resources.MergedDictionaries[0].Add("MetroColorFeature", Color.FromRgb(0xff, 0, 0));
            //Resources.MergedDictionaries[0].Add("MetroColorFeatureFade", Color.FromRgb(0, 0xff, 0));
  //          Resources.MergedDictionaries.Insert(0, LoadAccent("red"));
        }
        
        ResourceDictionary LoadAccent(string color)
        {
            var themepath = "/Themes/Accents/" + color + ".xaml";
            return LoadComponent(new Uri(themepath, UriKind.Relative)) as ResourceDictionary;
        }
    }

}
