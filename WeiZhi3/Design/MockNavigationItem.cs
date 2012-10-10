using Weibo.ViewModels;

namespace WeiZhi3.Design
{
    internal class MockNavigationItem : NavigationItem
    {
        public MockNavigationItem():base("x")
        {
            text = "如需心理咨询，教育咨询，情感援助，人才测评";
            image = "http://tp2.sinaimg.cn/1983918185/180/5641289122/1";
        }

        #region Overrides of NavigationItem

        public override void OnTick(IWeiboAccessToken at)
        {
            
        }

        #endregion
    }
}