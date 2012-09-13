using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiZhi3.DataModel;

namespace WeiZhi3.ViewModel
{
    class ViewModelLocator
    {
        private Profile _profile = Profile.Load();
        public Profile Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }
    }
}
