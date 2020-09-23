using System;
using System.Threading.Tasks;
using Listen.Models.RealmAccess;
using Listen.Models.Tasks;
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
        public async Task<Token> GetTokenAsync(string email, string password)
        {
            return await TokenWS.Instance.GetTokenAsync(email, password);
        }

        public async Task<string> GetTokenAsync()
        {
            var user = await UserManager.Instance.GetUserAsync();
            var token = user?.Token;
            // -- On  checke si Token valide
            var infos = await GetInfoAsync(token);
            if (infos == null)
            {
                var newtoken = await RefreshTokenAsync(user?.RefreshToken);
                if(newtoken != null) { 
                    return newtoken?.AccessToken;
                }
            }
            return token;
        }

        public async Task<Token> RefreshTokenAsync(string refresh_token)
        {
            var rt = refresh_token;
            if (string.IsNullOrEmpty(rt))
            {
                var user = await UserManager.Instance.GetUserAsync();
                rt = user?.RefreshToken;
            }

            var newtoken = await TokenWS.Instance.RefreshTokenAsync(rt);
            if (newtoken != null) {
                // -- Update User in DB
                await UserRealm.Instance.UpdateTokenAsync(newtoken);
            }
            return newtoken;
        }

        public async Task<TokenInfo> GetInfoAsync(string token)
        {
            return await TokenWS.Instance.GetInfoAsync(token);
        }
    }
}
