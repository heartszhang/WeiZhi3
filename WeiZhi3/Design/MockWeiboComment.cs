using System;
using Weibo.ViewModels;

namespace WeiZhi3.Design
{
    internal class MockWeiboComment:WeiboComment
    {
        public MockWeiboComment()
        {
            user = new DesignUserExt();
            text = "����û���Ա�����һ��Ӧ��----����Լ���ˣ� ";
            created_at = DateTime.Now;
        }
    }
}