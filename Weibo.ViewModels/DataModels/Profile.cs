using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using Weibo.Api2.Sina;

namespace Weibo.ViewModels.DataModels
{
    
    [DataContract]
    public class Profile
    {
        [DataMember]
        public int Magic { get; set; }

        [DataMember]
        public Account[] Accounts { get; set; }

        public bool IsEmpty()
        {
            return Accounts == null || Accounts.Length == 0;
        }
        public Account Account()
        {
            if (Accounts == null || Accounts.Length == 0)
                return null;
            return Accounts[0];
        }
        public async Task VerifyAccounts()
        {
            if(Accounts == null || Accounts.Length == 0)
                return ;
            var wr = new Task[Accounts.Length];

            var i = 0;
            foreach(var a in Accounts)
            {
//todo: check weibo sources here
                var a1 = a;
                wr[i++] =  SinaClient.rate_limit_status(a.AccessToken).ContinueWith(
                    (t)=>
                        {
                            if (t.Result.Failed())
                            {
                                var err = string.Format("id:{2} rate_limit_status:{0}:{1}", t.Result.Error(), t.Result.Reason(), a1.Id);
                                Debug.WriteLine(err);
                                Messenger.Default.Send(new NotificationMessage(err),"rate_limit_status");//user-name required
                                
                                a1.Id = 0;
                            }
                        });
            }
            await Task.WhenAll(wr);
            Remove(0);//delete all error accounts
            //Messenger.Default.Send(wrs,"rate_limit_status");
            return;
        }
        public long Id()
        {
            if (Accounts == null || Accounts.Length == 0)
                return 0;
            return Accounts[0].Id;
        }
        public string Token(long uid = 0)
        {
            if (Accounts == null || Accounts.Length == 0)
                return null;
            return uid == 0 ? Accounts[0].AccessToken : (from a in Accounts where a.Id == uid select a.AccessToken).FirstOrDefault();
        }
        public void Add(string token, long expiredseconds,long uid)
        {
            if(Accounts != null)
            {
                foreach(var a in Accounts)
                {
                    if(a.Id == uid)
                    {
                        a.Expired = DateTime.Now.UnixTimestamp() + expiredseconds;
                        a.AccessToken = token;
                    }
                }
                return;
            }
            var als = new List<Account>();
            if (Accounts != null)
                als.AddRange(Accounts);
            
            als.Add(new Account {AccessToken = token, Expired = DateTime.Now.UnixTimestamp() +expiredseconds,Id = uid});
            Accounts = als.ToArray();
        }

        public void Remove(string token)
        {
            if (Accounts == null)
                return;
            var als = new List<Account>();
            if (Accounts != null)
                als.AddRange(Accounts.Where(ac => ac.AccessToken != token));
            Accounts = als.ToArray();
        }

        public void Remove(long id)
        {
            if (Accounts == null)
                return;
            var acs = Accounts.ToList();
            var r = acs.RemoveAll(a => a.Id == id);
            if (r > 0)
            {
                Accounts = acs.ToArray();
            }
        }

        public void Clean()
        {
            if (Accounts == null)
                return;
            var ut = DateTime.Now.UnixTimestamp();
            var als = Accounts.ToList();
            if (als.RemoveAll(a => a.Expired < ut) > 0)
            {
                Accounts = als.ToArray();
            }

        }

        public void Save()
        {
            var profilep = ProfilePath();
            var ser = new DataContractJsonSerializer(typeof (Profile));
            using (var stream = new FileStream(profilep, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                ser.WriteObject(stream, this);
            }
        }

        public static Profile Load()
        {
            var profilep = ProfilePath();
            if (!File.Exists(profilep))
                return new Profile();
            var ser = new DataContractJsonSerializer(typeof (Profile));
            using (var stream = File.OpenRead(profilep))
            {
                var fdi = ser.ReadObject(stream) as Profile;
                if (fdi != null)
                    fdi.Clean();
                return fdi;
            }
            //Debug.WriteLine(profilep);
        }

        public static string ProfilePath()
        {
            return Path.Combine(UserRoot(), ".accounts");
        }

        public static string UserRoot()
        {
            var r = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var root = r + @"\WeiZhi\";
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            return root;
        }


    }
}
