using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Listen.Models.RealmAccess;
using Listen.Models.WebServices;

namespace Listen.Managers
{
    public class ServerManager
    {
        private static readonly Lazy<ServerManager> lazy = new Lazy<ServerManager>(() => new ServerManager());

        public ServerManager()
        {
        }

        public static ServerManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task<IList<Models.RealmObjects.Survey>> GetSurveysAsync()
        {
            // -- On  checke si Token valide
            string token = await TokenManager.Instance.GetTokenAsync();

            //token = null; // -- TEST

            if (token != null)
            {
                var list = await SurveyWS.Instance.GetSurveysAsync(token);
                await SurveyManager.Instance.AddOrUpdateAsync(list);
                var _list = await SurveyRealm.Instance.GetSurveysAsync();
                _list = _list.OrderByDescending(o => o.Type == "national").ToList();
                return _list;
            }
            return null;
        }

        public async Task<UserInfos> GetUserInfosAsync()
        {
            // -- On  checke si Token valide
            string token = await TokenManager.Instance.GetTokenAsync();

            if (token != null)
            {
                var infos = await UserWS.Instance.GetUserInfosAsync(token);
                await UserRealm.Instance.AddOrUpdateAsync(infos?.LastName, infos?.FirstName, infos?.EmailAddress, infos?.Country, infos?.ZipCode, infos?.Uuid, null, null);
                return infos;
            }
            return null;
        }

        public async Task UploadRepliesAsync()
        {
            var list = await SurveyRealm.Instance.GetRepliesAsync();

            if (list.Count() == 0)
            {
                return;
            }

            // -- On  checke si Token valide
            string token = await TokenManager.Instance.GetTokenAsync();

            if (token != null)
            {
                await SurveyWS.Instance.PostRepliesAsync(list, token);
            }
        }
    }
}

