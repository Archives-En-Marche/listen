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
            return await SurveyRealm.Instance.GetSurveysAsync();
        }

        public async Task AddOrUpdateAsync(IList<Models.WebServices.Survey> surveys)
        {
            await SurveyRealm.Instance.AddOrUpdateAsync(surveys);
        }
    }
}
