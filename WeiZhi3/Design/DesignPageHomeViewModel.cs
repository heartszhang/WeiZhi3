using Weibo.ViewModels;

namespace WeiZhi3.Design
{
    internal class DesignPageHomeViewModel:PageHomeViewModel
    {
        public DesignPageHomeViewModel() : base(0,string.Empty)
        {
            user = new DesignUserExt();
        }
    }
    internal class MockWeiboEditViewModel : WeiboEditViewModel
    {
        
    }
}