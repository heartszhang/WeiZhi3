using System;
using System.Linq;
using Weibo.DataModel;
using Weibo.ViewModels;

namespace WeiZhi3.Design
{
    internal class MockWeiboComment:WeiboComment
    {
        public MockWeiboComment()
        {
            user = new MockUserExt();
            text = "今天没带冈本！第一反应是----不能约炮了！ ";
            created_at = DateTime.Now;
            status = new Status() { user = new User { screen_name = "what'syouname" }, text = "-不能约炮了！" };
            replier = new CommentReply(0,0);
        }
    }
}