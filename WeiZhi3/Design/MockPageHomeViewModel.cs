using Weibo.ViewModels;

namespace WeiZhi3.Design
{
    internal class MockPageHomeViewModel:PageHomeViewModel
    {
        public MockPageHomeViewModel() : base(0,string.Empty)
        {
            user = new MockUserExt();
        }

    }
}