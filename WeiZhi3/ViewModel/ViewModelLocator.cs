using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weibo.ViewModels;
using Weibo.ViewModels.DataModels;

namespace WeiZhi3.ViewModel
{
    //public class UserAccessToken:IWeiboAccessToken
    //{
    //    private readonly long _user_id;
    //    private readonly string _token;

    //    public UserAccessToken(long userId, string token)
    //    {
    //        _user_id = userId;
    //        _token = token;
    //    }

    //    #region Implementation of IWeiboAccessToken

    //    public string get()
    //    {
    //        return _token;
    //    }

    //    public long id()
    //    {
    //        return _user_id;
    //    }

    //    public int count_per_page()
    //    {
    //        return 50;
    //    }

    //    #endregion
    //}
    class ViewModelLocator 
    {
        static private Profile _profile = Profile.Load();
        
        public Profile Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }
        public IWeiboAccessToken AccessToken
        {
            get { return Profile.Account(); }    
        }
        public IWeiboAccessToken UserAccessToken(long uid)
        {
            return Profile.Account(uid);
        }
        #region Implementation of IWeiboAccessToken

        public string get(long userid)
        {
            return Profile.Token(userid);
        }

        public int count_per_page()
        {
            return 50;
        }

        #endregion
    }
}
