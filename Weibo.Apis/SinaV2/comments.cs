using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel;

namespace Weibo.Apis.SinaV2
{
    public partial class WeiboClient
    {
        public static async Task<RestResult<Comments>> comments_mentions_refresh_async(long sinceid,
            int count,
            int page, string token)
        {
            var path = string.Format("comments/mentions.json?since_id={0}&access_token={1}&count={2}&page={3}"
                                     , sinceid,
                                     token,
                                     count,
                                     page);
            return await WeiboInternal.HttpsGet<Comments>(WeiboSources.SinaV2(path));
        }

        public static async Task<RestResult<Comments>> comments_to_me_refresh_async(long sinceid,
            int count,
            int page, string token)
        {
            var path = string.Format("comments/to_me.json?since_id={0}&count={1}&page={2}&access_token={3}",
                                     sinceid,
                                     count,
                                     page, token);

            return await WeiboInternal.HttpsGet<Comments>(WeiboSources.SinaV2(path));
        }
        public static async Task<RestResult<Comments>> comments_timeline_refresh_async(long sinceid,
            int count,
            int page, string token)
        {
            var path = string.Format("comments/timeline.json?since_id={0}&count={1}&page={2}&access_token={3}",
                                     sinceid,
                                     count,
                                     page, token);

            return await WeiboInternal.HttpsGet<Comments>(WeiboSources.SinaV2(path));
        }
        public static async Task<RestResult<Comments>> comments_timeline_next_page_async(long maxid,
            int count,
            int page, string token)
        {
            var path = string.Format("comments/timeline.json?max_id={0}&count={1}&page={2}&access_token={3}",
                                     maxid,
                                     count,
                                     page,
                                     token);

            return await WeiboInternal.HttpsGet<Comments>(WeiboSources.SinaV2(path));
        }

        public static async Task<RestResult<Comment>> comments_reply_async(long cid
            , long statusid, string comment
            , int withoutmention, int commentori, string token)
        {
            var data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>( "access_token", token ), 
                new KeyValuePair<string, string>( "comment", comment ),
                new KeyValuePair<string, string>("id", statusid.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("cid", cid.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("without_mention", withoutmention.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("comment_ori", commentori.ToString(CultureInfo.InvariantCulture)),
            };
            return await WeiboInternal.HttpsPost<Comment>(WeiboSources.SinaV2("comments/reply.json"), data);
        }
        public static async Task<RestResult<Comment>> comments_create_async(string comment
            , long comment_status_id
            , bool comment_ori, string token)
        {
            var data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>( "access_token", token ), 
                new KeyValuePair<string, string>( "comment", comment ),
                new KeyValuePair<string, string>("id", comment_status_id.ToString(CultureInfo.InvariantCulture)),
            };
            if(comment_ori)
                data.Add(new KeyValuePair<string, string>("comment_ori", "1" ));
            return await WeiboInternal.HttpsPost<Comment>(WeiboSources.SinaV2("comments/create.json"), data);
        }
        public static async Task<RestResult<Comment>> comments_destroy(long cid, string token)
        {
            var path = string.Format("comments/destroy.json?cid={0}&access_token={1}", cid, token);
            return await WeiboInternal.HttpsPost<Comment>(WeiboSources.SinaV2(path), new List<KeyValuePair<string, string>>());
        }
        public static async Task<RestResult<Comments>> comments_show_refresh_async(long statusid, long sinceid
            , int count, int page, string token)
        {
            var path = string.Format("comments/show.json?id={0}&count={1}&page={2}&access_token={3}&since_id={4}"
                , statusid, count, page, token, sinceid);
            return await WeiboInternal.HttpsGet<Comments>(WeiboSources.SinaV2(path));
        }
        public static async Task<RestResult<Comments>> comments_show_next_page_async(long statusid, long maxid
            , int count, int page, string token)
        {
            var path = string.Format("comments/show.json?id={0}&count={1}&page={2}&access_token={3}&max_id={4}", statusid, count, page, token, maxid);
            return await WeiboInternal.HttpsGet<Comments>(WeiboSources.SinaV2(path));
        }
    }
}
