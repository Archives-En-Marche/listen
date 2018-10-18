using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Listen.Helpers;
using Listen.Models.WebServices;
using Listen.Models.RealmObjects;
using PopolLib.Extensions;
using Realms;
using SurveyRealmObject = Listen.Models.RealmObjects.Survey;
using Newtonsoft.Json;

namespace Listen.Models.RealmAccess
{
    public class SurveyRealm
    {
        private static readonly Lazy<SurveyRealm> lazy = new Lazy<SurveyRealm>(() => new SurveyRealm());

        public SurveyRealm()
        {
        }

        public static SurveyRealm Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task<IList<SurveyRealmObject>> GetSurveysAsync()
        {
            return await Task<IList<SurveyRealmObject>>.Factory.StartNew(() =>
            {
                var db_name = Settings.AppSettings.GetValueOrDefault("DB_NAME", "");
                var realm = Realm.GetInstance(db_name);
                var all = realm.All<SurveyRealmObject>();
                return all.ToList().Clone();
            });
        }

        public async Task AddOrUpdateAsync(IList<WebServices.Survey> surveys)
        {
            var db_name = Settings.AppSettings.GetValueOrDefault("DB_NAME", "");
            var realm = Realm.GetInstance(db_name);

            var all = realm.All<SurveyRealmObject>();

            if (all?.Count() > 0)
            {
                await realm.WriteAsync(r =>
                {
                    r.RemoveAll<SurveyRealmObject>();
                });
            }

            if (surveys?.Count > 0)
            {
                foreach (var s in surveys)
                {
                    await realm.WriteAsync(r =>
                    {
                        var _s = new SurveyRealmObject()
                        {
                            Uuid = s.Uuid,
                            Name = s.Name,
                            Questions = JsonConvert.SerializeObject(s.Questions),
                        };
                        r.Add(_s);
                    });

                }
            }
        }
    }
}
