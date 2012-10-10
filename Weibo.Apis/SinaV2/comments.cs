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
        public enum comments_filter_by_source
        {
            all = 0,
            from_status = 1,
            from_group = 2
        }
        public static async Task<RestResult<Comments>> comments_by_me_next_page_async(string token,long maxid,
            int count,
            int page, comments_filter_by_source filter = comments_filter_by_source.all)
        {
            var path = string.Format("comments/by_me.json?max_id={0}&count={1}&page={2}&access_token={3}&filter_by_source={4}",
                                     maxid,
                                     count,
                                     page, token,(int)filter);

            return await WeiboInternal.HttpsGet<Comments>(WeiboSources.SinaV2(path));
        }
        public static async Task<RestResult<Comments>> comments_by_me_refresh_async(string token,long sinceid,
            int count,
            int page, comments_filter_by_source filter = comments_filter_by_source.all)
        {
            var path = string.Format("comments/by_me.json?since_id={0}&count={1}&page={2}&access_token={3}",
                                     sinceid,
                                     count,
                                     page, token);

            return await WeiboInternal.HttpsGet<Comments>(WeiboSources.SinaV2(path));
        }
        public enum comments_filter_by_author
        {
            all = 0,
            from_followed = 1,
            from_stranger = 2
        }
        public static async Task<RestResult<Comments>> comments_to_me_next_page_async(string token,long maxid,
            int count,
            int page, comments_filter_by_author author_filter = comments_filter_by_author.all,
            comments_filter_by_source source_filter = comments_filter_by_source.all)
        {
            var path = string.Format("comments/to_me.json?max_id={0}&count={1}&page={2}&access_token={3}&filter_by_author={4}&filter_by_source={5}",
                                     maxid,
                                     count,
                                     page, token, (int)author_filter, (int)source_filter);

            return await WeiboInternal.HttpsGet<Comments>(WeiboSources.SinaV2(path));
        }
        public static async Task<RestResult<Comments>> comments_to_me_refresh_async(string token,long sinceid,
            int count,
            int page, comments_filter_by_author author_filter = comments_filter_by_author.all,
            comments_filter_by_source source_filter = comments_filter_by_source.all)
        {
            var path = string.Format("comments/to_me.json?since_id={0}&count={1}&page={2}&access_token={3}&filter_by_author={4}&filter_by_source={5}",
                                     sinceid,
                                     count,
                                     page, token,(int)author_filter,(int)source_filter);

            return await WeiboInternal.HttpsGet<Comments>(WeiboSources.SinaV2(path));
        }
        public static async Task<RestResult<Comments>> comments_timeline_refresh_async(string token,long sinceid,
            int count,
            int page, bool trim_user= false)
        {
            var path = string.Format("comments/timeline.json?since_id={0}&count={1}&page={2}&access_token={3}&trim_user={4}",
                                     sinceid,
                                     count,
                                     page, token,trim_user ? 1: 0);

            return await WeiboInternal.HttpsGet<Comments>(WeiboSources.SinaV2(path));
        }
        public static async Task<RestResult<Comments>> comments_timeline_next_page_async(string token,long maxid,
            int count,
            int page, bool trim_user = false)
        {
            var path = string.Format("comments/timeline.json?max_id={0}&count={1}&page={2}&access_token={3}&trim_user={4}",
                                     maxid,
                                     count,
                                     page,
                                     token,trim_user ? 1: 0);

            return await WeiboInternal.HttpsGet<Comments>(WeiboSources.SinaV2(path));
        }

        public static async Task<RestResult<Comment>> comments_reply_async(long cid
            , long statusid, string comment
            , bool withoutmention, bool commentori, string token)
        {
            var data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>( "access_token", token ), 
                new KeyValuePair<string, string>( "comment", comment ),
                new KeyValuePair<string, string>("id", statusid.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("cid", cid.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("without_mention", withoutmention?"0":"1"),
                new KeyValuePair<string, string>("comment_ori", commentori ? "1":"0"),
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
