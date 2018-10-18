using System;
using System.Linq;
using System.Threading.Tasks;
using Listen.Helpers;
using Listen.Models.RealmObjects;
using Listen.Models.WebServices;
using Realms;

namespace Listen.Models.RealmAccess
{
    public class UserRealm
    {
        private static readonly Lazy<UserRealm> lazy = new Lazy<UserRealm>(() => new UserRealm());

        public UserRealm()
        {
        }

        public static UserRealm Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task AddOrUpdateAsync(string lastname, string firstname, string phone, string mail, string token, string refreshtoken)
        {
            var db_name = Settings.AppSettings.GetValueOrDefault("DB_NAME", "");
            var realm = Realm.GetInstance(db_name);

            var users = realm.All<User>();

            if (users.Count() == 0) // -- ADD
            {
                await realm.WriteAsync(r =>
                {
                    var u = new User()
                    {
                        LastName = lastname,
                        FirstName = firstname,
                        Phone = phone,
                        Mail = mail,
                        Token = token,
                        RefreshToken = refreshtoken,
                        LastAccess = DateTimeOffset.Now
                    };

                    r.Add(u);
                });
            }
            else if (users.Count() == 1) // -- UPDATE
            {
                await realm.WriteAsync(r =>
                {
                    var _users = r.All<User>();
                    var current = _users.First();
                    current.LastName = lastname;
                    current.FirstName = firstname;
                    current.Phone = phone;
                    current.Mail = mail;
                    current.Token = token;
                    current.RefreshToken = refreshtoken;
                    current.LastAccess = DateTimeOffset.Now;
                });
            }
        }

        public async Task UpdateTokenAsync(Token token)
        {
            var db_name = Settings.AppSettings.GetValueOrDefault("DB_NAME", "");
            var realm = Realm.GetInstance(db_name);

            var users = realm.All<User>();
            var user = users.FirstOrDefault();
            if (token != null && user != null)
            {
                await realm.WriteAsync(r =>
                {
                    var _users = r.All<User>();
                    var current = _users.First();
                    current.Token = token.AccessToken;
                    current.RefreshToken = token.RefreshToken;
                    current.LastAccess = DateTimeOffset.Now;
                });
            }
        }

        public async Task<User> GetUser()
        {
            return await Task.Factory.StartNew(() =>
            {
                var db_name = Settings.AppSettings.GetValueOrDefault("DB_NAME", "");
                using (var realm = Realm.GetInstance(db_name))
                {
                    var users = realm.All<User>();
                    var user = users.FirstOrDefault();
                    return user?.Clone();
                }
            });
        }
    }
}
