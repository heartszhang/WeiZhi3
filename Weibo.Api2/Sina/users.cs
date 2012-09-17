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
screen_name	false	string	需要查询的用户昵称。 * */
        public static async Task<WeiboResponse> users_show(string access_token, long uid = 0, string screen_name = null)
        {
            dynamic paras = new ExpandoObject();
            paras.access_token = access_token;
            if (uid == 0)
                paras.screen_name = screen_name;
            else paras.uid = uid;

            return await WeiboClient.WeiboGet(new WeiboRequestHandler("users/show.json", paras), WeiboSources.Sina);
        }
    }
}
