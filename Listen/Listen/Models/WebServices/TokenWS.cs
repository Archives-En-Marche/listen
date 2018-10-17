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
    public class TokenWS
    {
        private static readonly Lazy<TokenWS> lazy = new Lazy<TokenWS>(() => new TokenWS());

        public TokenWS()
        {
        }

        public static TokenWS Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task<TokenInfo> GetInfoAsync(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var base_url = Settings.AppSettings.GetValueOrDefault("WS_BASE_URL", "");
                var timeout = Settings.AppSettings.GetValueOrDefault("WS_TIME_OUT", 0);
                var client = new RestClient(base_url);
                var request = new RestRequest("/oauth/v2/tokeninfo?access_token=" + token, Method.GET);

                var cts = new CancellationTokenSource(timeout);
                var result = await client.ExecuteTaskAsync(request, cts.Token);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<TokenInfo>(result.Content);
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

        public async Task<Token> RefreshTokenAsync(string refresh_token)
        {
            if (!string.IsNullOrEmpty(refresh_token))
            {
                var base_url = Settings.AppSettings.GetValueOrDefault("WS_BASE_URL", "");
                var timeout = Settings.AppSettings.GetValueOrDefault("WS_TIME_OUT", 0);
                var client = new RestClient(base_url);
                var request = new RestRequest("/oauth/v2/token", Method.POST);
                request.AddParameter("client_id", "2b494496-4eae-4946-9ae1-efa3f593595c");
                request.AddParameter("client_secret", "vcyrJys1sdvaTCXN0BFfOuw2A8KxdA9QkYDMErViM68=");
                request.AddParameter("grant_type", "refresh_token");
                request.AddParameter("refresh_token", refresh_token);
                var cts = new CancellationTokenSource(timeout);
                var result = await client.ExecuteTaskAsync(request, cts.Token);
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<Token>(result.Content); ;
                }
                else
                {
                    return null;
                }
                    ;
            }
            else
            {
                return null;
            }
        }
    }

    public class TokenInfo
    {
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "grant_types")]
        public List<string> GrantTypes { get; set; }

        [JsonProperty(PropertyName = "scopes")]
        public List<string> Scopes { get; set; }
    }

    public class Token
    {
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }
    }
}
