using System;
using System.Threading.Tasks;
using Listen.Models.RealmAccess;
using Listen.Models.RealmObjects;
using Listen.Models.WebServices;

namespace Listen.Managers
{
    public class UserManager
    {
        private static readonly Lazy<UserManager> lazy = new Lazy<UserManager>(() => new UserManager());

        public UserManager()
        {
        }

        public static UserManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task<User> GetUserAsync()
        {
            return await UserRealm.Instance.GetUser();
        }

        public async Task AddOrUpdateAsync(string lastname, string firstname, string mail, string country, string zipcode, string uuid, string token, string refreshtoken)
        {
            await UserRealm.Instance.AddOrUpdateAsync(lastname, firstname, mail, country, zipcode, uuid, token, refreshtoken);
        }

        public async Task UpdateTokenAsync(Token token)
        {
            await UserRealm.Instance.UpdateTokenAsync(token);
        }

        public async Task DeleteUserAsync()
        {
            await UserRealm.Instance.DeleteUserAsync();
        }
    }
}
