using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weibo.ViewModels;
using Weibo.ViewModels.DataModels;

namespace WeiZhi3.ViewModel
{
    class ViewModelLocator : IWeiboAccessToken
    {
        static private Profile _profile = Profile.Load();
        
        public Profile Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }
        public IWeiboAccessToken AccessToken
        {
            get { return this; }    
        }

        #region Implementation of IWeiboAccessToken

        string IWeiboAccessToken.get()
        {
            return Profile.Token();
        }

        long IWeiboAccessToken.id()
        {
            return Profile.Id(0);
        }

        #endregion
    }
}
