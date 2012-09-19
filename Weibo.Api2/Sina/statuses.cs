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
        /*statuses/home_timeline
         * source	false	string	采用OAuth授权方式不需要此参数，其他授权方式为必填参数，数值为应用的AppKey。
access_token	false	string	采用OAuth授权方式为必填参数，其他授权方式不需要此参数，OAuth授权后获得。
since_id	false	int64	若指定此参数，则返回ID比since_id大的微博（即比since_id时间晚的微博），默认为0。
max_id	false	int64	若指定此参数，则返回ID小于或等于max_id的微博，默认为0。
count	false	int	单页返回的记录条数，默认为50。
page	false	int	返回结果的页码，默认为1。
base_app	false	int	是否只获取当前应用的数据。0为否（所有数据），1为是（仅当前应用），默认为0。
feature	false	int	过滤类型ID，0：全部、1：原创、2：图片、3：视频、4：音乐，默认为0。
trim_user	false	int	返回值中user字段开关，0：返回完整user字段、1：user字段仅返回user_id，默认为0。
         * */
        public static async Task<WeiboResponse> home_timeline(string access_token, 
            long since_id = 0, long max_id = 0, int count = 50, int page = 1, int base_app = 0, int feature = 0, int trim_user = 0)
        {
            dynamic paras = new ExpandoObject();
            paras.access_token = access_token;
            paras.since_id = since_id;
            paras.max_id = max_id;
            paras.count = count;
            paras.page = page;
            paras.base_app = base_app;
            paras.feature = feature;
            paras.trim_user = trim_user;
            return await WeiboClient.WeiboGet(new WeiboRequestHandler("statuses/home_timeline.json", paras), WeiboSources.Sina);
        }

        /*statuses/friends_timeline/ids
source	false	string	采用OAuth授权方式不需要此参数，其他授权方式为必填参数，数值为应用的AppKey。
access_token	false	string	采用OAuth授权方式为必填参数，其他授权方式不需要此参数，OAuth授权后获得。
since_id	false	int64	若指定此参数，则返回ID比since_id大的微博（即比since_id时间晚的微博），默认为0。
max_id	false	int64	若指定此参数，则返回ID小于或等于max_id的微博，默认为0。
count	false	int	单页返回的记录条数，默认为50。
page	false	int	返回结果的页码，默认为1。
base_app	false	int	是否只获取当前应用的数据。0为否（所有数据），1为是（仅当前应用），默认为0。
feature	false	int	过滤类型ID，0：全部、1：原创、2：图片、3：视频、4：音乐，默认为0。
         * */
        public static async Task<WeiboResponse> friends_timeline_ids(string access_token, long since_id = 0, long max_id = 0, int count = 50, int page = 1, int base_app = 0, int feature = 0)
        {
            dynamic paras = new ExpandoObject();
            paras.access_token = access_token;
            paras.since_id = since_id;
            paras.max_id = max_id;
            paras.count = count;
            paras.page = page;
            paras.base_app = base_app;
            paras.feature = feature;
            return await WeiboClient.WeiboGet(new WeiboRequestHandler("statuses/friends_timeline/ids.json", paras), WeiboSources.Sina);
        }

    }
}
