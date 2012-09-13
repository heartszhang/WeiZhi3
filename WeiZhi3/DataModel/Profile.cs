using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace WeiZhi3.DataModel
{
    [DataContract]
    public class Account
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string AccessToken { get; set; }

        [DataMember]
        public long Expired { get; set; }
    }

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

        public void Add(string token, long expired)
        {
            var als = new List<Account>();
            if (Accounts != null)
                als.AddRange(Accounts);
            als.Add(new Account {AccessToken = token, Expired = expired});
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
