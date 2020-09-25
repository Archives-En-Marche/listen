using System;
using System.Diagnostics;
using System.Linq;
using GalaSoft.MvvmLight;
using Listen.Managers;
using Listen.ViewModels;
using Listen.VisualElements;
using Microsoft.AppCenter.Crashes;
using PopolLib.Services;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class HomePage : BaseContentPage
    {
        INavigation _nav;

        public HomePage(INavigation navigation, ViewModelBase vm)
        {
            _nav = navigation;
            //this.Title = "Accueil";
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "");
            BindingContext = vm;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                // -- On charge les questionnaires
                //hud.Show("Chargement ...");
                //Token newtoken;
                var user = await UserManager.Instance.GetUserAsync();

                //await ServerManager.Instance.GetSurveysAsync();

                var displayLoginPage = false;

                if (user != null && !string.IsNullOrEmpty(user?.Token) && !string.IsNullOrEmpty(user?.RefreshToken))
                {
                    var token = user?.Token;
                    var refresh = user?.RefreshToken;

                    //// -- Check if TOKEN not expire
                    //var infos = await TokenWS.Instance.GetInfoAsync(token);
                    //if (infos == null)
                    //{
                    //    displayLoginPage = true;
                    //}
                    //else

                    // -- On REFRESH AUTO Le TOKEN ?
                    await TokenManager.Instance.GetTokenAsync();

                    //var infos = await TokenWS.Instance.GetInfoAsync(newtoken?.AccessToken);
                    //if (infos == null)
                    //{
                    //    displayLoginPage = true;
                    //}
                    //else
                    {
                        LongRunningTaskManager.Instance.StartLongRunningTask();
                    }
                }
                else
                {
                    displayLoginPage = true;
                }

                if (displayLoginPage)
                {
                    // -- On présente la page de login
                    var nav = ((NavigationPage)Application.Current.MainPage).Navigation;
                    var mstack = nav.ModalStack;
                    if (mstack.FirstOrDefault(p => p is InternalNavigationPage) == null)
                    {
                        await ((NavigationPage)Application.Current.MainPage).Navigation.PushModalAsync(new InternalNavigationPage(new LoginPage(new LoginPageViewModel(_nav))));
                    }
                }

                // -- Check App Version
                var remoteAppVersion = await AppVersionManager.Instance.GetAppVersionAsync();
                var need = await AppVersionManager.Instance.IsUpdateAppNeededAsync();
                //Debug.WriteLine(remoteAppVersion.ToString());
                if (need)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var dialog = DependencyService.Get<IDialogService>();
                        dialog.Show("Mise à Jour", "Une mise à jour de l'application est nécessaire. En cliquant sur OK, vous allez être redirigé sur la page de mise à jour de l'application.", "OK", (res) =>
                        {
                            if (res)
                            {
                                if (Device.RuntimePlatform == Device.iOS)
                                {
                                    var ass = DependencyService.Get<IAppStoreService>();
                                    ass.OpenAppInStore("id1438099575");
                                }
                                else
                                {
                                    var ass = DependencyService.Get<IAppStoreService>();
                                    ass.OpenAppInStore("fr.en-marche.listen");
                                }
                            }
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine(ex.Message);
            }
            finally
            {
            }

        }
    }
}
