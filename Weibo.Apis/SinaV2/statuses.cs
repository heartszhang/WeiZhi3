using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Weibo.Apis.Net;
using Weibo.DataModel;

namespace Weibo.Apis.SinaV2
{
    public partial class WeiboClient
    {
        public static async Task<RestResult<Statuses>> statuses_friends_timeline_refresh_async(string token,long sinceid = 0, int page = 1, int count = 50)
        {
            var path = string.Format("statuses/friends_timeline.json?count={0}&since_id={1}&access_token={2}&page={3}"
                , count, sinceid, token, page);
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(path));
        }

        public static async Task<RestResult<Statuses>> statuses_friends_timeline_next_page_async(string token, int page = 1, long maxid = 0, int count = 50 )
        {
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(
                string.Format("statuses/friends_timeline.json?count={0}&page={1}&feature={2}&max_id={3}&access_token={4}", 
                count, page, 0, maxid, token)));
        }

        public static async Task<RestResult<Statuses>> statuses_mentions_refresh_async(string token, long sinceid,
            int page = 1, int count = 50
            )
        {
            var path = string.Format("statuses/mentions.json?page={0}&count={1}&since_id={2}&access_token={3}",
                                     page,
                                     count,
                                     sinceid,
                                     token);
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(path));
        }
        public static async Task<RestResult<StatusIds>> statuses_mentions_ids_async(string token,long sinceid, int count )
        {
            var path = string.Format("statuses/mentions/ids.json?since_id={0}&access_token={1}&count={2}", sinceid, token, count);
            return await WeiboInternal.HttpsGet<StatusIds>(WeiboSources.SinaV2(path));
        }
        public static async Task<RestResult<StatusIds>> statuses_friends_timeline_ids_async( string token,long sinceid, int page, int count)
        {
            var path = string.Format("statuses/friends_timeline/ids.json?since_id={0}&access_token={1}&count={2}&page={3}", sinceid, token, count, page);
            return await WeiboInternal.HttpsGet<StatusIds>(WeiboSources.SinaV2(path));
        }
        public static async Task<RestResult<Statuses>>
            statuses_hot_repost_daily_async(string token,int count )
        {
            var path = string.Format("statuses/hot/repost_daily.json?count={0}&access_token={1}", count, token);
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(path));
        }

        public static async Task<RestResult<Statuses>>
            statuses_hot_repost_repost_weekly_async(int count, string token)
        {
            var path = string.Format("statuses/hot/repost_weekly.json?count={0}&access_token={1}", count, token);
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(path));
        }

        public static async Task<RestResult<Statuses>>
            statuses_hot_comments_daily(int count, string token)
        {
            var path = string.Format("statuses/hot/comments_daily.json?count={0}&access_token={1}"
                , count, token);
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(path));
        }

        public static async Task<RestResult<Statuses>>
            statuses_hot_comments_weekly(int count, string token)
        {
            var path = string.Format("statuses/hot/comments_weekly.json?count={0}&access_token={1}"
                , count, token);
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(path));
        }
        //no api
        public static async Task<RestResult<Statuses>>
            statuses_search_async(string q, long count, long page, string token, long consumerkey)
        {
            var path = string.Format("statuses/search.json?q={0}&page={1}&count={2}&source={3}", Uri.EscapeDataString(q), page, count, consumerkey);
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(path));
        }
        //static async Task<RestResult<Status[]>> LoadRepostStatusAsync(string path)
        //{
        //    var r = await WeiboInternal.HttpsGet<Reposts>(WeiboSources.SinaV2(path));
        //    var rtn = new RestResult<Status[]>
        //    {
        //        StatusCode = r.StatusCode,
        //        Reason = r.Reason,
        //        Value = r.Value == null ? null : r.Value.reposts,
        //    };
        //    return rtn;
        //}

        public static async Task<RestResult<Reposts>>
            statuses_repost_timeline_refresh_async(string token,long statusid, long sinceid, int page, int count )
        {
            var path = string.Format("statuses/repost_timeline.json?id={0}&since_id={1}&count={2}&page={3}&access_token={4}",
                statusid, sinceid, count, page, token);
            return await WeiboInternal.HttpsGet<Reposts>(WeiboSources.SinaV2(path));
        }
        public static async Task<RestResult<Reposts>>
            statuses_repost_timeline_next_page_async(string token,long statusid, long maxid, int page, int count )
        {
            var path = string.Format("statuses/repost_timeline.json?id={0}&max_id={1}&count={2}&page={3}&access_token={4}",
                statusid, maxid, count, page, token);

            return await WeiboInternal.HttpsGet<Reposts>(WeiboSources.SinaV2(path));
        }
        public static async Task<RestResult<Status>>
            statuses_upload_async(string filepath, string status, string access_token)
        {
            string path = string.Format("statuses/upload.json?access_token={0}", access_token);
            var rtn = new RestResult<Status>();
            using (var client = new HttpClient())
            {
                var filec = new StreamContent(File.OpenRead(filepath));
                var form = new MultipartFormDataContent
                    {
                        {new StringContent(access_token), "access_token"},
                        {new StringContent(status), "status"},
                        {filec, "pic", "weizhi.jpg"}
                    };

                var resp = await client.PostAsync(WeiboSources.SinaUpload(path), form);
                rtn.StatusCode = resp.StatusCode;
                rtn.Reason = resp.ReasonPhrase;

                if (resp.IsSuccessStatusCode)
                    rtn.Value = JsonConvert.DeserializeObject<Status>(await resp.Content.ReadAsStringAsync());
                else if ((int)resp.StatusCode >= 400)
                {
                    var er = JsonConvert.DeserializeObject<WeiboError>(await resp.Content.ReadAsStringAsync());
                    rtn.Reason = er.Translate();
                }
            }
            return rtn;
        }

        public static async Task<RestResult<Status>> statuses_update_async(string token,string post, long replyto )
        {
            var data = new NameValueCollection { { "access_token", token }, { "status", post } };
            return await WeiboInternal.HttpsPost<Status>("statuses/update.json", data);
        }

        public static async Task<RestResult<Status>> statuses_repost_async(string post
            , long statusid, int iscomment, string token)
        {
            var data = new NameValueCollection
            {
                { "access_token", token },
                {"id", statusid.ToString(CultureInfo.InvariantCulture)},
            };
            if (!string.IsNullOrEmpty(post))
            {
                data.Add("status", post);
                data.Add("is_comment", iscomment.ToString(CultureInfo.InvariantCulture));
            }
            return await WeiboInternal.HttpsPost<Status>("statuses/repost.json", data);
        }

        public static async Task<RestResult<Status>> statuses_destroy_async(long statusid, string token)
        {
            var path = string.Format("statuses/destroy.json?access_token={0}&id={1}", token, statusid);
            return await WeiboInternal.HttpsPost<Status>(path, new NameValueCollection());
        }
        public static async Task<RestResult<Statuses>> statuses_public_timeline_async(long consumerkey
            , int count, int page, int base_app = 0)
        {
            var path = string.Format("statuses/public_timeline.json?source={0}&count={1}&page={2}&base_app={3}"
                , consumerkey, count, page, base_app);
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(path));
        }

        public static async Task<RestResult<Statuses>>
            statuses_user_timeline_refresh_async(string token,long userid, long sinceid, int count, int page )
        {
            var path = String.Format("statuses/user_timeline.json?uid={0}&since_id={1}&count={2}&page={3}&access_token={4}"
                , userid, sinceid, count, page, token);
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(path));
        }

        public static async Task<RestResult<Statuses>>
            statuses_user_timeline_next_page_async(string token,long userid, long maxid, int page, int count )
        {
            var path = String.Format("statuses/user_timeline.json?uid={0}&max_id={1}&count={2}&page={3}&access_token={4}", userid, maxid, count, page, token);
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(path));
        }

        public static async Task<RestResult<Statuses>>
            statuses_user_timeline_refresh_async(string token,string screenname, long sinceid, int count, int page )
        {
            var path = String.Format("statuses/user_timeline.json?uid={0}&since_id={1}&count={2}&page={3}&access_token={4}"
                , screenname, sinceid, count, page, token);
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(path));
        }

        public static async Task<RestResult<Statuses>>
            statuses_user_timeline_next_page_async(string token,string screenname, long maxid, int page, int count )
        {
            var path = String.Format("statuses/user_timeline.json?uid={0}&max_id={1}&count={2}&page={3}&access_token={4}", screenname, maxid, count, page, token);
            return await WeiboInternal.HttpsGet<Statuses>(WeiboSources.SinaV2(path));
        }
    }
}
