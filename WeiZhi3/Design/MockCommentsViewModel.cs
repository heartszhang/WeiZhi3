using Weibo.ViewModels;

namespace WeiZhi3.Design
{
    internal class MockCommentsViewModel : CommentsViewModel
    {
        public MockCommentsViewModel() : base(0)
        {
            comments.Add(new MockWeiboComment());
            comments.Add(new MockWeiboComment());
        }
    }
}