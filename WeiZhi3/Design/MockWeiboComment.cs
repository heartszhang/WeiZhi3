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
            text = "����û���Ա�����һ��Ӧ��----����Լ���ˣ� ";
            created_at = DateTime.Now;
            status = new Status() { user = new User { screen_name = "what'syouname" }, text = "-����Լ���ˣ�" };
            replier = new CommentReply(0,0);
        }
    }
}