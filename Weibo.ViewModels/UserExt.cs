using System;
using Newtonsoft.Json.Linq;
using Weibo.DataModel;

namespace Weibo.ViewModels
{
    public class UserExt : ObservableObjectExt
    {
        public string profile_image_url { get; set; }
        public string screen_name { get; set; }
        public bool verified { get; set; }
        public long id { get; set; }
        public string name { get; set; }
        public long province { get; set; }
        public long city { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string domain { get; set; }
        public string gender { get; set; }//m/f
        public int followers_count { get; set; }
        public int friends_count { get; set; }
        public int statuses_count { get; set; }
        public int favourites_count { get; set; }
        public DateTime created_at { get; set; }

        private bool _follow_me;
        private bool _following;

        public bool allow_all_act_msg { get; set; }
        public bool geo_enabled { get; set; }
        public string latest_status { get; set; }
        public int bi_followers_count { get; set; }
        public int online_status { get; set; }

        public string verified_reason { get; set; }
        public int verified_type { get; set; }

        public string avatar_large { get; set; }
        public bool allow_all_comment { get; set; }
        public string remark { get; set; }

        public bool follow_me { get { return _follow_me; } set { Set(ref _follow_me, value); } }
        public bool following { get { return _following; } set { Set(ref _following, value); } }
        public long status_id { get; set; }

        public void assign_sina(UserWithoutStatus data)
        {
            /*
id	int64	用户UID
screen_name	string	用户昵称
name	string	友好显示名称
province	int	用户所在地区ID
city	int	用户所在城市ID
location	string	用户所在地
description	string	用户描述
url	string	用户博客地址
profile_image_url	string	用户头像地址
domain	string	用户的个性化域名
gender	string	性别，m：男、f：女、n：未知
followers_count	int	粉丝数
friends_count	int	关注数
statuses_count	int	微博数
favourites_count	int	收藏数
created_at	string	创建时间
following	boolean	当前登录用户是否已关注该用户
allow_all_act_msg	boolean	是否允许所有人给我发私信
geo_enabled	boolean	是否允许带有地理信息
verified	boolean	是否是微博认证用户，即带V用户
allow_all_comment	boolean	是否允许所有人对我的微博进行评论
avatar_large	string	用户大头像地址
verified_reason	string	认证原因
follow_me	boolean	该用户是否关注当前登录用户
online_status	int	用户的在线状态，0：不在线、1：在线
bi_followers_count	int	用户的互粉数
status             */
            if (data == null)
                return;
            bi_followers_count = data.bi_followers_count;
            online_status = data.online_status;
            follow_me = data.follow_me;
            verified_reason = data.verified_reason;
            avatar_large = data.avatar_large;
            allow_all_comment = data.allow_all_comment;
            verified = data.verified;
            geo_enabled = data.geo_enabled;
            allow_all_act_msg = data.allow_all_act_msg;
            following = data.following;
            created_at = WeiboStatus.time(data.created_at.ToString());//data.created_at;
            url = data.url;
            profile_image_url = data.profile_image_url;
            domain = data.domain;
            gender = data.gender;
            followers_count = data.followers_count;
            friends_count = data.friends_count;
            statuses_count = data.statuses_count;
            favourites_count = data.favourites_count;
            description = data.description;
            id = data.id;
            screen_name = data.screen_name;
            name = data.name;
            province = data.province;
            city = data.city;
            location = data.location;

        }
    }
}