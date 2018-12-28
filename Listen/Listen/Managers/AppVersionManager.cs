using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Listen.Helpers;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using PopolLib.Enums;
using PopolLib.Models;
using PopolLib.Services;
using RestSharp;
using Xamarin.Forms;

namespace Listen.Managers
{
    public class AppVersionManager
    {
        private static readonly Lazy<AppVersionManager> lazy = new Lazy<AppVersionManager>(() => new AppVersionManager());

        public AppVersionManager()
        {
        }

        public static AppVersionManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public async Task<AppVersionResult> GetAppVersionAsync()
        {
            try
            {
                var base_url = Settings.AppSettings.GetValueOrDefault("WS_BASE_URL", "");
                var scope = Settings.AppSettings.GetValueOrDefault("APP_VERSION_SCOPE", "");

                var timeout = Settings.AppSettings.GetValueOrDefault("WS_TIME_OUT", 30000);
                var client = new RestClient(base_url);

                var request = new RestRequest("versions.json", Method.GET);

                var cts = new CancellationTokenSource(timeout);
                var result = await client.ExecuteTaskAsync<List<AppVersionResult>>(request, cts.Token);

                return result.Data?.FirstOrDefault(s => s.Scope == scope);
            }
            catch (OperationCanceledException ex)
            {
                Crashes.TrackError(ex);
                throw new Exception("TIME_OUT");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Crashes.TrackError(ex);
                throw ex;
            }
        }

        public async Task<bool> IsUpdateAppNeededAsync()
        {
            var di = DependencyService.Get<IDeviceInfos>();
            var deviceV = di.GetAppVersion();
            var appStoreV = await this.GetAppVersionAsync();

            if (appStoreV != null)
            {
                AppUpdateType update;

                if (Device.RuntimePlatform == Device.iOS)
                {
                    update = await AppVersion.IsUpdateAppNeededAsync(appStoreV.iOS, deviceV);
                }
                else
                {
                    update = await AppVersion.IsUpdateAppNeededAsync(appStoreV.Droid, deviceV);
                }

                if (update == AppUpdateType.Major || update == AppUpdateType.Minor)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    public class AppVersionResult
    {
        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("ios")]
        public string iOS { get; set; }

        [JsonProperty("droid")]
        public string Droid { get; set; }
    }
}

