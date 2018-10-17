using System;
using System.Threading.Tasks;
using Listen.Models.RealmAccess;

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

        public async Task AddOrUpdateAsync(string lastname, string firstname, string phone, string mail, string token, string refreshtoken)
        {
            await UserRealm.Instance.AddOrUpdateAsync(lastname, firstname, phone, mail, token, refreshtoken);
        }
    }
}
