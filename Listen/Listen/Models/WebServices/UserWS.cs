using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Listen.Helpers;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using RestSharp;

namespace Listen.Models.WebServices
{
    public class UserWS
    {
        private static readonly Lazy<UserWS> lazy = new Lazy<UserWS>(() => new UserWS());

        public UserWS()
        {
        }

        public static UserWS Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task<UserInfos> GetUserInfosAsync(string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var base_url = Settings.AppSettings.GetValueOrDefault("WS_BASE_URL", "");
                    var timeout = Settings.AppSettings.GetValueOrDefault("WS_TIME_OUT", 0);
                    var client = new RestClient(base_url);
                    var request = new RestRequest("/api/me", Method.GET);
                    request.AddHeader("Authorization", "Bearer " + token);
                    var cts = new CancellationTokenSource(timeout);
                    var result = await client.ExecuteAsync(request, cts.Token);

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<UserInfos>(result.Content);
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
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                return null;
            }
        }
    }

    public class UserInfos
    {

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
}
