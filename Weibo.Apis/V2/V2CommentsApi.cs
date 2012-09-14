using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel.Comment;
using Weibo.DataModel.CommentModel;

namespace Weibo.Apis.V2
{
    public partial class Weibo
    {
        public static async Task<RestResult<Comment[]>> CommentsMentionsRefrshAsync(long sinceid,
            int count,
            int page,string token)
        {
            var path = string.Format("comments/mentions.json?since_id={0}&access_token={1}&count={2}&page={3}"
                                     ,sinceid,
                                     token,
                                     count,
                                     page);
            return await LoadCommentsAsync(path);
        }

        public static async Task<RestResult<Comment[]>> CommentsMentionsRefreshAsync(long sinceid,
            int count,
            int page, string token)
        {
            var path = string.Format("comments/mentions.json?since_id={0}&access_token={1}&count={2}&page={3}"
                                     ,sinceid,
                                     token,
                                     count,
                                     page);
            return await LoadCommentsAsync(path);
        }
        public static async Task<RestResult<Comment[]>> Comments2MeRefreshAsync(long sinceid,
            int count,
            int page, string token)
        {
            var path = string.Format("comments/to_me.json?since_id={0}&count={1}&page={2}&access_token={3}",
                                     sinceid,
                                     count,
                                     page,token);

            return await LoadCommentsAsync(path);
        }
        public static async Task<RestResult<Comment[]>> CommentsTimelineRefreshAsync(long sinceid,
            int count,
            int page,string token)
        {
            var path = string.Format("comments/timeline.json?since_id={0}&count={1}&page={2}&access_token={3}",
                                     sinceid,
                                     count,
                                     page,token);

            return await LoadCommentsAsync(path);
        }
        public static async Task<RestResult<Comment[]>> CommentsTimelineNextPageAsync(long maxid,
            int count,
            int page,string token)
        {
            var path = string.Format("comments/timeline.json?max_id={0}&count={1}&page={2}&access_token={3}",
                                     maxid,
                                     count,
                                     page,
                                     token);

            return await LoadCommentsAsync(path);
        }

        public static async Task<RestResult<Comment>> CommentsReplyAsync(long cid
            , long statusid, string comment
            , int withoutmention, int commentori,string token)
        {
            var data = new NameValueCollection
            {
                { "access_token", token }, 
                { "comment", comment },
                {"id", statusid.ToString(CultureInfo.InvariantCulture)},
                {"cid", cid.ToString(CultureInfo.InvariantCulture)},
                {"without_mention", withoutmention.ToString(CultureInfo.InvariantCulture)},
                {"comment_ori", commentori.ToString(CultureInfo.InvariantCulture)},
            };
            return await HttpsPost<Comment>("comments/reply.json", data);
        }
        public static async Task<RestResult<Comment>> CommentsCreateAsync(string comment
            , long comment_status_id
            , long reply_to_id
            , int comment_ori, long without_mention, string token)
        {
            var data = new NameValueCollection
            {
                { "access_token", token }, 
                { "comment", comment },
                {"id", comment_status_id.ToString(CultureInfo.InvariantCulture)},
            };
            if (comment_ori != -1)
            {
                data.Add("comment_ori", comment_ori.ToString(CultureInfo.InvariantCulture));
            }
            return await HttpsPost<Comment>("comments/create.json", data);
        }
        public static async Task<RestResult<Comment>> CommentsDestroy(long cid, string token)
        {
            var path = string.Format("comments/destroy.json?cid={0}&access_token={1}", cid, token);
            return await HttpsPost<Comment>(path, new NameValueCollection());
        }
        public static async Task<RestResult<Comment[]>> CommentsShowRefreshAsync(long statusid, long sinceid
            , int count, int page, string token)
        {
            var path = string.Format("comments/show.json?id={0}&count={1}&page={2}&access_token={3}&since_id={4}"
                , statusid, count, page, token, sinceid);
            return await LoadCommentsAsync(path);
        }
        public static async Task<RestResult<Comment[]>> CommentsShowNextPageAsync(long statusid, long maxid
            , int count, int page, string token)
        {
            var path = string.Format("comments/show.json?id={0}&count={1}&page={2}&access_token={3}&max_id={4}", statusid, count, page, token, maxid);
            return await LoadCommentsAsync(path);
        }
        public static async Task<RestResult<Comment[]>> LoadCommentsAsync(string path)
        {
            var r = await HttpsGet<Comments>(path);

            return new RestResult<Comment[]>(r)
            {
                Value = r.Value != null ? r.Value.comments : null
            };
        }

    }
}
