using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Listen.Helpers;
using Listen.Managers;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using RestSharp;

namespace Listen.Models.WebServices
{
    public class SurveyWS
    {
        private static readonly Lazy<SurveyWS> lazy = new Lazy<SurveyWS>(() => new SurveyWS());

        public SurveyWS()
        {
        }

        public static SurveyWS Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task<List<Survey>> GetSurveysAsync(string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var base_url = Settings.AppSettings.GetValueOrDefault("WS_BASE_URL", "");
                    var timeout = Settings.AppSettings.GetValueOrDefault("WS_TIME_OUT", 0);
                    var client = new RestClient(base_url);
                    var request = new RestRequest("/api/jecoute/survey", Method.GET);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var cts = new CancellationTokenSource(timeout);
                    var result = await client.ExecuteTaskAsync(request, cts.Token);

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<List<Survey>>(result.Content);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                Crashes.TrackError(ex);
                return null;
            }
        }

        public async Task PostRepliesAsync(IList<RealmObjects.Reply> list, string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var base_url = Settings.AppSettings.GetValueOrDefault("WS_BASE_URL", "");
                    var timeout = Settings.AppSettings.GetValueOrDefault("WS_TIME_OUT", 0);
                    var client = new RestClient(base_url);

                    Debug.WriteLine("Token : " + token);

                    foreach (var s in list)
                    {
                        var request = new RestRequest("/api/jecoute/survey/reply", Method.POST);
                        request.AddHeader("Authorization", "Bearer " + token);
                        request.AddHeader("Accept", "application/json");
                        request.AddHeader("Content-Type", "application/json");
                        request.RequestFormat = DataFormat.Json;
                        var cts = new CancellationTokenSource(timeout);
                        request.AddParameter("text/plain", s.Answer, ParameterType.RequestBody);
                        //request.AddJsonBody(s.Answer);

                        var result = await client.ExecuteTaskAsync(request, cts.Token);
                        if (result.StatusCode == HttpStatusCode.Created)
                        {
                            // -- on update la bdd
                            await SurveyManager.Instance.SetUploaded(s, true);
                            Debug.WriteLine("Success : " + s.Answer);
                        }
                        else
                        {
                            Debug.WriteLine("KO : " + s.Answer);
                        }

                    }
                }
                else
                {
                    Debug.WriteLine("KO : Empty Token");
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }
    }

    public class Choice
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class Question
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("choices")]
        public IList<Choice> Choices { get; set; }
    }

    public class Survey
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("questions")]
        public IList<Question> Questions { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Reply
    {
        [JsonProperty("survey")]
        public string Survey { get; set; }

        [JsonProperty("lastName")]
        public string Lastmame { get; set; }

        [JsonProperty("firstName")]
        public string Firstname { get; set; }

        [JsonProperty("emailAddress")]
        public string Email { get; set; }

        [JsonProperty("agreedToStayInContact")]
        public bool AgreedToStayInContact { get; set; }

        [JsonProperty("agreedToContactForJoin")]
        public bool AgreedToContactForJoin { get; set; }

        [JsonProperty("agreedToTreatPersonalData")]
        public bool AgreedToTreatPersonalData { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("ageRange")]
        public string AgeRange { get; set; }

        [JsonProperty("profession")]
        public string Profession { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("genderOther")]
        public string GenderOther { get; set; }

        [JsonProperty("answers")]
        public IList<Answer> Answers { get; set; }
    }

    public class Answer
    {
        [JsonProperty("surveyQuestion")]
        public string SurveyQuestion { get; set; }

        [JsonProperty("textField")]
        public string TextField { get; set; }

        [JsonProperty("selectedChoices")]
        public IList<string> SelectedChoices { get; set; }
    }
}
