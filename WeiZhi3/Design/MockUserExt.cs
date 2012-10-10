using Weibo.ViewModels;

namespace WeiZhi3.Design
{
    internal class MockUserExt : UserExt
    {
        public MockUserExt()
        {
            screen_name = "钥匙就在那阳光";
            profile_image_url = "http://tp2.sinaimg.cn/1680241201/50/5636852916/1";
            avatar_large = "http://tp2.sinaimg.cn/1983918185/180/5641289122/1";
            followers_count = 21323213;
            friends_count = 3;
            statuses_count = 123232131;
            description = "一枚摄影控，一枚鞋子控，一枚已婚小青年！";
            verified_reason = "如需心理咨询，教育咨询，情感援助，人才测评";
        }
    }
}