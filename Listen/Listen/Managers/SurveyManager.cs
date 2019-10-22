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

        //public async Task<IList<Survey>> GetSurveysAsync()
        //{
        //    await ServerManager.Instance.GetSurveysAsync();
        //    var list = await SurveyRealm.Instance.GetSurveysAsync();
        //    if (list?.Count == 0)
        //    {
        //        // -- On charge les surveys
        //        await ServerManager.Instance.GetSurveysAsync();
        //        var _list = await SurveyRealm.Instance.GetSurveysAsync();
        //        _list = _list.OrderByDescending(o => o.Type == "national").ToList();
        //        MessagingCenter.Send<SurveyManager, IList<Survey>>(this, "UpdateUI", _list);
        //        return _list;
        //    }
        //    else
        //    {
        //        list = list.OrderByDescending(o => o.Type == "national").ToList();
        //        MessagingCenter.Send<SurveyManager, IList<Survey>>(this, "UpdateUI", list);
        //        return list;
        //    }
        //}

        public async Task AddOrUpdateAsync(IList<Models.WebServices.Survey> surveys) => await SurveyRealm.Instance.AddOrUpdateAsync(surveys);

        public async Task AddReplyAsync(Models.WebServices.Reply reply) => await SurveyRealm.Instance.AddReplyAsync(reply);

        public async Task SetUploaded(Reply reply, bool uploaded) => await SurveyRealm.Instance.SetUploaded(reply, uploaded);
    }
}
