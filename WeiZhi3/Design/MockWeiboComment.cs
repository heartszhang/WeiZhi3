using System;
using Weibo.ViewModels;

namespace WeiZhi3.Design
{
    internal class MockWeiboComment:WeiboComment
    {
        public MockWeiboComment()
        {
            user = new DesignUserExt();
            text = "今天没带冈本！第一反应是----不能约炮了！ ";
            created_at = DateTime.Now;
        }
    }
}