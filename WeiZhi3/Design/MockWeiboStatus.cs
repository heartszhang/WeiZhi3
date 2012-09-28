using Weibo.ViewModels;

namespace WeiZhi3.Design
{
    internal class MockWeiboStatus : WeiboStatus
    {
        public MockWeiboStatus()
        {
            user = new DesignUserExt();
            text = " 第21届联合国人权大会于9月18号在日内瓦万国宫举行。两个国际非政府组织提出要求联合国紧急调查中国法轮功学员器官被活摘和盗卖这一滔天罪行。与此同时，中共卫生部副部长黄杰夫接受«新世纪»采访，声称要建立一个符合普世的伦理价值和符合中国国情的可持续发展的器官捐赠系统。分析人士认为，活摘法轮功学员器官直接冲击中共执政合法性，因此黄杰夫受命于中共出来做个姿态，企图混淆视听，替中共政权逃脱罪责";
            topic = "联合国关注活摘器官 黄洁夫欺骗国际社会";
            bmiddle_pic = "http://ww2.sinaimg.cn/bmiddle/61d83ed4jw1dx38yf6k9ij.jpg";
        }
    }
}