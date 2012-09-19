using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Weibo.ViewModels
{
    public class ObservableObjectExt : ObservableObject
    {//只有vs11支持 callermembername
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string property_name = null)
        {
            Set(property_name, ref storage, value);
            return true;
        }

    }
}
