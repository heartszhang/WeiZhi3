using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weibo.ViewModels;

namespace WeiZhi3.Design
{
    internal class DesignDialogMessageBox
    {
        public string Title { get { return "Hello"; } }
        public string Message { get { return "Dialog Message "; } }
    }
    internal class DesignUserExt : UserExt
    {
        public DesignUserExt()
        {
            screen_name = "钥匙就在那阳光";
        }
    }
    internal class DesignPageHomeViewModel:PageHomeViewModel
    {
        public DesignPageHomeViewModel() : base(0,string.Empty)
        {
            user = new DesignUserExt();
        }
    }
}
