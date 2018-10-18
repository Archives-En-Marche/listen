using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Listen.Helpers;
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
            if (!string.IsNullOrEmpty(token))
            {
                var base_url = Settings.AppSettings.GetValueOrDefault("WS_BASE_URL", "");
                var timeout = Settings.AppSettings.GetValueOrDefault("WS_TIME_OUT", 0);
                var client = new RestClient(base_url);
                var request = new RestRequest("/api/jecoute/survey", Method.GET);
                request.AddHeader("Authorization", "Beaver: " + token);
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
    }

    public class Choice
    {

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class Question
    {

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("choices")]
        public IList<Choice> Choices { get; set; }
    }

    public class Survey
    {

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("questions")]
        public IList<Question> Questions { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
