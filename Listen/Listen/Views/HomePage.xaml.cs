using System;
using System.Collections.Generic;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using Listen.Managers;
using Listen.Models.WebServices;
using Listen.ViewModels;
using Listen.VisualElements;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class HomePage : ContentPage
    {
        INavigation _nav;

        public HomePage(INavigation navigation, ViewModelBase vm)
        {
            _nav = navigation;
            this.Title = "Accueil";
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

                var user = await UserManager.Instance.GetUserAsync();

                ServerManager.Instance.GetSurveysAsync();

                var displayLoginPage = false;

                if (user != null)
                {
                    var token = user.Token;
                    var refresh = user?.RefreshToken;

                    // -- Check if TOKEN not expire
                    var infos = await TokenWS.Instance.GetInfoAsync(token);
                    if (infos == null)
                    {
                        displayLoginPage = true;
                    }
                    else
                    {
                        // -- Refresh TOKEN ?
                        var newtoken = await TokenWS.Instance.RefreshTokenAsync(refresh);
                        await UserManager.Instance.AddOrUpdateAsync(null, null, null, null, null, null, newtoken?.AccessToken, newtoken?.RefreshToken);
                    }
                }
                else
                {
                    displayLoginPage = true;
                }

                if (displayLoginPage)
                {
                    // -- On présente la page de login
                    await ((NavigationPage)Application.Current.MainPage).Navigation.PushModalAsync(new InternalNavigationPage(new LoginPage(new LoginPageViewModel(_nav))));
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //hud.Dismiss();
            }
            finally
            {
                //hud.Dismiss();
                //if (Application.Current.Properties.ContainsKey("FirstUse"))
                //{
                //    //Do things when it's NOT the first use...
                //}
                //else
                //{
                //    //Do things when it IS the first use...
                //    await ((NavigationPage)MainPage).Navigation.PushModalAsync(new InternalNavigationPage(new ParametresPage(new ParametresPageViewModel(this))));
                //}

                //var permissions = DependencyService.Get<IPermissions>();
                //await permissions.RequestPermissionsAsync();
            }

        }
    }
}
