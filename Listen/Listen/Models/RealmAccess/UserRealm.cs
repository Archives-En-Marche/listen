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

        public async Task AddOrUpdateAsync(string lastname, string firstname, string mail, string country, string zipcode, string uuid, string token, string refreshtoken)
        {
            var db_name = Settings.AppSettings.GetValueOrDefault("DB_NAME", "");
            var realm = Realm.GetInstance(db_name);

            var users = realm.All<User>();
            if (users.Count() == 0) // -- ADD
            {
                await realm.WriteAsync(r =>
                {
                    var u = new User();
                    if (!string.IsNullOrEmpty(lastname))
                    {
                        u.LastName = lastname;
                    }
                    if (!string.IsNullOrEmpty(firstname))
                    {
                        u.FirstName = firstname;
                    }
                    if (!string.IsNullOrEmpty(country))
                    {
                        u.Country = country;
                    }
                    if (!string.IsNullOrEmpty(zipcode))
                    {
                        u.ZipCode = zipcode;
                    }
                    if (!string.IsNullOrEmpty(uuid))
                    {
                        u.Uuid = uuid;
                    }
                    if (!string.IsNullOrEmpty(mail))
                    {
                        u.Mail = mail;
                    }
                    if (!string.IsNullOrEmpty(token))
                    {
                        u.Token = token;
                    }
                    if (!string.IsNullOrEmpty(refreshtoken))
                    {
                        u.RefreshToken = refreshtoken;
                    }
                    u.LastAccess = DateTimeOffset.Now;

                    r.Add(u);
                });
            }
            else if (users.Count() == 1) // -- UPDATE
            {
                await realm.WriteAsync(r =>
                {
                    var _users = r.All<User>();
                    var current = _users.First();
                    if (!string.IsNullOrEmpty(lastname))
                    {
                        current.LastName = lastname;
                    }
                    if (!string.IsNullOrEmpty(firstname))
                    {
                        current.FirstName = firstname;
                    }
                    if (!string.IsNullOrEmpty(country))
                    {
                        current.Country = country;
                    }
                    if (!string.IsNullOrEmpty(zipcode))
                    {
                        current.ZipCode = zipcode;
                    }
                    if (!string.IsNullOrEmpty(uuid))
                    {
                        current.Uuid = uuid;
                    }
                    if (!string.IsNullOrEmpty(mail))
                    {
                        current.Mail = mail;
                    }
                    if (!string.IsNullOrEmpty(token))
                    {
                        current.Token = token;
                    }
                    if (!string.IsNullOrEmpty(refreshtoken))
                    {
                        current.RefreshToken = refreshtoken;
                    }
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

        public async Task<User> GetUserAsync()
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

        public User GetUser()
        {
            var db_name = Settings.AppSettings.GetValueOrDefault("DB_NAME", "");
            using (var realm = Realm.GetInstance(db_name))
            {
                var users = realm.All<User>();
                var user = users.FirstOrDefault();
                return user?.Clone();
            }
        }

        public async Task DeleteUserAsync()
        {
            var db_name = Settings.AppSettings.GetValueOrDefault("DB_NAME", "");
            var realm = Realm.GetInstance(db_name);
            await realm.WriteAsync(r =>
            {
                r.RemoveAll<User>();
            });
        }
    }
}
