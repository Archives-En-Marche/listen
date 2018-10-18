using System;
using System.Threading.Tasks;
using Listen.Models.RealmAccess;
using Listen.Models.WebServices;

namespace Listen.Managers
{
    public class TokenManager
    {
        private static readonly Lazy<TokenManager> lazy = new Lazy<TokenManager>(() => new TokenManager());

        public TokenManager()
        {
        }

        public static TokenManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task<Token> RefreshTokenAsync(string refresh_token)
        {
            var newtoken = await TokenWS.Instance.RefreshTokenAsync(refresh_token);

            // -- Update User in DB
            await UserRealm.Instance.UpdateTokenAsync(newtoken);

            return newtoken;
        }

        public async Task<TokenInfo> GetInfoAsync(string token)
        {
            return await TokenWS.Instance.GetInfoAsync(token);
        }
    }
}
