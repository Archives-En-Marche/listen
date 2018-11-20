﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Listen.Models.RealmAccess;
using Listen.Models.WebServices;
using Listen.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;

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

        public async Task<List<Survey>> GetSurveysAsync()
        {
            string token = "";
            var user = await UserManager.Instance.GetUserAsync();
            token = user?.Token;
            // -- On  checke si Token valide
            var infos = await TokenManager.Instance.GetInfoAsync(token);
            if (infos == null)
            {
                var newtoken = await TokenManager.Instance.RefreshTokenAsync(user?.RefreshToken);
                token = newtoken?.AccessToken;
            }
            var list = await SurveyWS.Instance.GetSurveysAsync(token);
            await SurveyManager.Instance.AddOrUpdateAsync(list);
            MessagingCenter.Send<ServerManager, IList<Survey>>(this, "UpdateUI", list);
            return list;
        }

        public async Task<UserInfos> GetUserInfosAsync(string token)
        {
            var infos = await UserWS.Instance.GetUserInfosAsync(token);
            await UserRealm.Instance.AddOrUpdateAsync(infos?.LastName, infos?.FirstName, infos?.EmailAddress, infos?.Country, infos?.ZipCode, infos?.Uuid, null, null);
            return infos;
        }

        public async Task UploadRepliesAsync()
        {
            var list = await SurveyRealm.Instance.GetRepliesAsync();
            var user = await UserRealm.Instance.GetUserAsync();
            var infos = await TokenWS.Instance.GetInfoAsync(user?.Token);

            var newToken = await TokenManager.Instance.RefreshTokenAsync(user?.RefreshToken);
            await SurveyWS.Instance.PostRepliesAsync(list, newToken?.AccessToken);
        }
    }
}
