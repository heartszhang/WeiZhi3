using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;

namespace Weibo.ViewModels
{
    public abstract class ViewModelBase2 : ViewModelBase
    {
        protected bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            Set(propertyName, ref storage, value);
            return true;
        }
    }
}