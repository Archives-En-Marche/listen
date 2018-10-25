using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using Listen.Helpers;
using Listen.Models.RealmObjects;
using Realms;
using PopolLib.Extensions;
using Listen.Models.RealmAccess;
using Xamarin.Forms;

namespace Listen.Managers
{
    public class SurveyManager
    {
        private static readonly Lazy<SurveyManager> lazy = new Lazy<SurveyManager>(() => new SurveyManager());

        public SurveyManager()
        {
        }

        public static SurveyManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task<IList<Survey>> GetSurveysAsync()
        {
            var list = await SurveyRealm.Instance.GetSurveysAsync();
            if (list?.Count == 0)
            {
                // -- On chage les surveys
                await ServerManager.Instance.GetSurveysAsync();
                return await SurveyRealm.Instance.GetSurveysAsync();
            }
            else
            {
                MessagingCenter.Send<SurveyManager, IList<Survey>>(this, "UpdateUI", list);
                return list;
            }
        }

        public async Task AddOrUpdateAsync(IList<Models.WebServices.Survey> surveys)
        {
            await SurveyRealm.Instance.AddOrUpdateAsync(surveys);
        }
    }
}
