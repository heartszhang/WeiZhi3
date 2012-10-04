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
        private static readonly ColorAccent[] _accents= new ColorAccent[] 
            {
                Accent(Color.FromRgb(0x00,0x82,0x99), Color.FromRgb(0x00,0xA0,0xB1)),//Teal
                Accent(Color.FromRgb(0x26,0x72,0xEC), Color.FromRgb(0x2E,0x8D,0xEF)),//Blue
                Accent(Color.FromRgb(0x8C,0x00,0x95), Color.FromRgb(0x8C,0x00,0x95)),//Purple
                Accent(Color.FromRgb(0x51,0x33,0xAB), Color.FromRgb(0x51,0x33,0xAB)),//Dark Purple
                Accent(Color.FromRgb(0xAC,0x19,0x3D), Color.FromRgb(0xBF,0x1E,0x4B)),//Red
                Accent(Color.FromRgb(0xD2,0x47,0x26), Color.FromRgb(0xDC,0x57,0x2E)),//Orange
                Accent(Color.FromRgb(0x00,0x8A,0x00), Color.FromRgb(0x00,0xA6,0x00)),//Green
                Accent(Color.FromRgb(0x09,0x4A,0xB2), Color.FromRgb(0x0A,0x5B,0xC4)),//Sky Blue
                Accent(Color.FromRgb(0x55,0x55,0x55), Color.FromRgb(0x66,0x66,0x66)),//Silver
                Accent(Color.FromRgb(0x8e,0xbc,0x00), Color.FromRgb(0xa0,0xb9,0x55)),//Lime

                Accent(Color.FromRgb(0xE5,0x6C,0x19), Color.FromRgb( 0xFF,0x98,0x1D )),//Orange
                Accent(Color.FromRgb(0xB8,0x1B,0x1B), Color.FromRgb(0xFF,0x2E,0x12)),//Green
                Accent(Color.FromRgb(0xB8,0x1B,0x6C), Color.FromRgb( 0xFF,0x1D,0x77 )),//Sky Blue
                Accent(Color.FromRgb(0x69,0x1B,0xB8), Color.FromRgb(0xAA,0x40,0xFF)),//Silver
                Accent(Color.FromRgb(0x1B,0x58,0xB8), Color.FromRgb(0x1F,0xAE,0xFF)),//Lime
                Accent(Color.FromRgb(0x00,0xAA,0xAA), Color.FromRgb(0x00,0xD8,0xCC)),//Orange
                Accent(Color.FromRgb(0x83,0xBA,0x1F), Color.FromRgb(0x91,0xD1,0x00)),//Green
                Accent(Color.FromRgb(0xDE,0x4A,0xAD), Color.FromRgb(0xE7,0x73,0xBD)),//Sky Blue
                Accent(Color.FromRgb(0x00,0xA3,0xA3), Color.FromRgb( 0x43,0x9D,0x9A )),//Silver
                Accent(Color.FromRgb(0xFE,0x7C,0x22), Color.FromRgb(0xE5,0x6C,0x19)),//Lime
            };
        static App()
        {
            DispatcherHelper.Initialize();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var v = (DateTime.Now.Ticks + DateTime.Now.Ticks / 100000) % _accents.Length;
            Resources["MetroColorFeatureBrush"] = new SolidColorBrush(_accents[v].Feature);
            Resources["MetroColorFeatureFadeBrush"] = new SolidColorBrush(_accents[v].Light);
        }
        private static ColorAccent Accent(Color color1, Color color2)
        {
            return new ColorAccent() { Feature = color1, Light = color2 };

        }
        struct ColorAccent
        {
            public Color Feature;
            public Color Light;

        }
    }

}
