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
id	int64	�û�UID
screen_name	string	�û��ǳ�
name	string	�Ѻ���ʾ����
province	int	�û����ڵ���ID
city	int	�û����ڳ���ID
location	string	�û����ڵ�
description	string	�û�����
url	string	�û����͵�ַ
profile_image_url	string	�û�ͷ���ַ
domain	string	�û��ĸ��Ի�����
gender	string	�Ա�m���С�f��Ů��n��δ֪
followers_count	int	��˿��
friends_count	int	��ע��
statuses_count	int	΢����
favourites_count	int	�ղ���
created_at	string	����ʱ��
following	boolean	��ǰ��¼�û��Ƿ��ѹ�ע���û�
allow_all_act_msg	boolean	�Ƿ����������˸��ҷ�˽��
geo_enabled	boolean	�Ƿ�������е�����Ϣ
verified	boolean	�Ƿ���΢����֤�û�������V�û�
allow_all_comment	boolean	�Ƿ����������˶��ҵ�΢����������
avatar_large	string	�û���ͷ���ַ
verified_reason	string	��֤ԭ��
follow_me	boolean	���û��Ƿ��ע��ǰ��¼�û�
online_status	int	�û�������״̬��0�������ߡ�1������
bi_followers_count	int	�û��Ļ�����
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