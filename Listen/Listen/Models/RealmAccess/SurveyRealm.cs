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
using Reply = Listen.Models.WebServices.Reply;

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

        static string  db_name = Settings.AppSettings.GetValueOrDefault("DB_NAME", "");

        // -- Ajout du champs Type sur Survey => SchemaVersion = 1
        RealmConfiguration config = new RealmConfiguration(db_name)
        {
            SchemaVersion = 1
        };

        public async Task<IList<SurveyRealmObject>> GetSurveysAsync()
        {
            return await Task<IList<SurveyRealmObject>>.Factory.StartNew(() =>
            { 
                var realm = Realm.GetInstance(config);
                var all = realm.All<SurveyRealmObject>();
                return all.ToList().Clone();
            });
        }

        public async Task AddOrUpdateAsync(IList<WebServices.Survey> surveys)
        {
            var realm = Realm.GetInstance(config);

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
                            Uuid = s.Id,
                            Name = s.Name,
                            Type = s.Type,
                            Questions = JsonConvert.SerializeObject(s.Questions),
                        };
                        r.Add(_s);
                    });

                }
            }
        }

        public async Task AddReplyAsync(Reply reply)
        {
            var realm = Realm.GetInstance(config);

            await realm.WriteAsync(r =>
            {
                var _r = new RealmObjects.Reply()
                {
                    SurveyId = reply.Survey,
                    Date = DateTimeOffset.Now,
                    Uploading = false,
                    Answer = JsonConvert.SerializeObject(reply)
                };
                r.Add(_r);
            });
        }

        public async Task<IList<RealmObjects.Reply>> GetRepliesAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
               var realm = Realm.GetInstance(config);
                var list = realm.All<RealmObjects.Reply>().Where(r => r.Uploading == false).ToList();
                return list.Clone();
            });
        }

        public async Task SetUploaded(Models.RealmObjects.Reply reply, bool uploaded)
        {
            var realm = Realm.GetInstance(config);

            await realm.WriteAsync(r =>
            {
                var _r = r.All<Models.RealmObjects.Reply>().Where(i => i.Id == reply.Id).FirstOrDefault();
                if (_r != null)
                {
                    _r.Uploading = uploaded;
                    r.Add(_r);
                }
            });
        }

    }
}
