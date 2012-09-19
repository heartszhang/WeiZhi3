using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weibo.Api2.Sina
{
    public partial class SinaClient
    {
        /*users/show
        source	false	string	采用OAuth授权方式不需要此参数，其他授权方式为必填参数，数值为应用的AppKey。
access_token	false	string	采用OAuth授权方式为必填参数，其他授权方式不需要此参数，OAuth授权后获得。
uid	false	int64	需要查询的用户ID。
screen_name	false	string	需要查询的用户昵称。
         * {
    "id": 1404376560,
    "screen_name": "zaku",
    "name": "zaku",
    "province": "11",
    "city": "5",
    "location": "北京 朝阳区",
    "description": "人生五十年，乃如梦如幻；有生斯有死，壮士复何憾。",
    "url": "http://blog.sina.com.cn/zaku",
    "profile_image_url": "http://tp1.sinaimg.cn/1404376560/50/0/1",
    "domain": "zaku",
    "gender": "m",
    "followers_count": 1204,
    "friends_count": 447,
    "statuses_count": 2908,
    "favourites_count": 0,
    "created_at": "Fri Aug 28 00:00:00 +0800 2009",
    "following": false,
    "allow_all_act_msg": false,
    "geo_enabled": true,
    "verified": false,
    "status": {
        "created_at": "Tue May 24 18:04:53 +0800 2011",
        "id": 11142488790,
        "text": "我的相机到了。",
        "source": "<a href="http://weibo.com" rel="nofollow">新浪微博</a>",
        "favorited": false,
        "truncated": false,
        "in_reply_to_status_id": "",
        "in_reply_to_user_id": "",
        "in_reply_to_screen_name": "",
        "geo": null,
        "mid": "5610221544300749636",
        "annotations": [],
        "reposts_count": 5,
        "comments_count": 8
    },
    "allow_all_comment": true,
    "avatar_large": "http://tp1.sinaimg.cn/1404376560/180/0/1",
    "verified_reason": "",
    "follow_me": false,
    "online_status": 0,
    "bi_followers_count": 215
}
         * 
         * */
        public static async Task<WeiboResponse> users_show(string access_token, long uid = 0, string screen_name = null)
        {
            dynamic paras = new ExpandoObject();
            paras.access_token = access_token;
            if (uid != 0)
                paras.uid = uid;
            else if (!string.IsNullOrEmpty(screen_name))
                paras.screen_name = screen_name;            

            return await WeiboClientInternal.WeiboGet(new WeiboRequestHandler("users/show.json", paras), WeiboSources.Sina);
        }

        /*
         account/rate_limit_status
{
    "ip_limit": 10000,
    "limit_time_unit": "HOURS",
    "remaining_ip_hits": 10000,
    "remaining_user_hits": 150,
    "reset_time": "2011-06-03 18:00:00",
    "reset_time_in_seconds": 1415,
    "user_limit": 150,
}
         * */
        public static async Task<WeiboResponse> rate_limit_status(string access_token)
        {
            dynamic paras = new ExpandoObject();
            paras.access_token = access_token;
            return await WeiboClientInternal.WeiboGet(WeiboRequestHandler.Create("account/rate_limit_status.json", paras));
        }
    }
}
