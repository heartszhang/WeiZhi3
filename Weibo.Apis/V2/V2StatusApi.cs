using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Weibo.Apis.Net;
using Weibo.DataModel.CommentModel;
using Weibo.DataModel.Status;

namespace Weibo.Apis.V2
{
    public partial class Weibo
    {        
        public static async Task<RestResult<StatusWithUser>> 
            StatusesUploadAsync(string imagepath, string text, string token)
        {
            string path = string.Format("statuses/upload.json?access_token={0}", token);

            return await UploadAsync<StatusWithUser>(GetRequestAddress(path, "https://upload.api.weibo.com/2")
                , new NameValueCollection{{"status",text}}, imagepath, "pic");            
        }

        public static async Task<RestResult<StatusWithUser>> StatusesUpdateAsync(string post, long replyto, string token)
        {
            var data = new NameValueCollection {{"access_token", token}, {"status", post}};
            return  await HttpsPost<StatusWithUser>("statuses/update.json", data);
        }
        
        public static async Task<RestResult<StatusWithUser>> StatusRepostAsync(string post
            , long statusid, int iscomment, string token)
        {
            var data = new NameValueCollection
            {
                { "access_token", token },
                {"id", statusid.ToString(CultureInfo.InvariantCulture)},
            };
            if(!string.IsNullOrEmpty(post))
            {
                data.Add("status",post);
                data.Add("is_comment", iscomment.ToString(CultureInfo.InvariantCulture));
            }
            return await HttpsPost<StatusWithUser>("statuses/repost.json", data);
        }
        public static async Task<RestResult<Comment>> CommentCreateAsync(string comment
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
        public static async Task<RestResult<StatusWithUser>> Destroy(long statusid, string token)
        {
            var path = string.Format("statuses/destroy.json?access_token={0}&id={1}", token, statusid);
            return await HttpsPost<StatusWithUser>(path, new NameValueCollection());
        }
        public static async Task<RestResult<StatusWithUser[]>> PublicTimeline(long consumerkey
            , int count, int page, int base_app = 0)
        {
            var path = string.Format("statuses/public_timeline.json?source={0}&count={1}&page={2}&base_app={3}"
                , consumerkey, count, page, base_app);
            return await GetStatusAsync(path);
        }
        protected static async Task<RestResult<StatusWithUser[]>> GetStatusAsync(string path)
        {
            var rs = await HttpsGet<Statuses>(path);
            if (rs.Value != null)
                Debug.WriteLine("load-status total:{0}", rs.Value.total_number);
            return new RestResult<StatusWithUser[]>
            {
                StatusCode = rs.StatusCode,
                Error = rs.Error,
                Value = rs.Value == null ? null : rs.Value.statuses,
            };
        }
    }
}