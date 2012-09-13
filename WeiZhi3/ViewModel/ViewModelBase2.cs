using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;

namespace WeiZhi3.ViewModel
{
    internal class ViewModelBase2 : ViewModelBase
    {
        public bool Set<TProperty>(ref TProperty storage, TProperty value, [CallerMemberName] string propname = null)
        {
            return Set(propname, ref storage, value);
        }
    }
    internal class ProfileViewModel :ViewModelBase2
    {
        
    }
}