using System;
using System.Diagnostics;
using Listen.Helpers;
using Listen.iOS.Renderers;
using Listen.OAuth;
using Listen.ViewModels;
using Listen.Views;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using OAuth2Authenticator = Listen.OAuth.OAuth2Authenticator;

//[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]
namespace Listen.iOS.Renderers
{
    public class LoginPageRenderer : PageRenderer
    {
        bool IsShown;

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!IsShown)
            {
                IsShown = true;

#if DEBUG
                var auth = new OAuth2Authenticator(
                    "2b494496-4eae-4946-9ae1-efa3f593595c",
                    "vcyrJys1sdvaTCXN0BFfOuw2A8KxdA9QkYDMErViM68=",
                    "jecoute_surveys",
                    new Uri("https://staging.en-marche.fr/oauth/v2/auth"),
                    new Uri("https://staging.en-marche.fr"),
                    new Uri("https://staging.en-marche.fr/oauth/v2/token"), null, false)
                {
                    ShowErrors = false,
                    AllowCancel = false,
                    Title = ""
                };
#else
                var auth = new OAuth2Authenticator(
                    "33f63935-0793-41b9-89a8-1d9bb74e5fe5",
                    "X5lVzYtkoVBotQM1pmty/8el3UGrRgIpfzqXeUL0jbY=",
                    "jecoute_surveys",
                    new Uri("https://en-marche.fr/oauth/v2/auth"),
                    new Uri("https://en-marche.fr"),
                    new Uri("https://en-marche.fr/oauth/v2/token"))
                {
                    ShowErrors = false,
                    AllowCancel = false,
                    Title = ""
                };
#endif

                auth.Completed += async (sender, eventArgs) =>
                {
                    if (eventArgs.IsAuthenticated)
                    {
                        var lp = Element as LoginPage;
                        if (lp != null)
                        {
                            var vm = lp.BindingContext as LoginPageViewModel;
                            if (vm != null)
                            {
                                await vm.AddOrUpdateTokenAsync(eventArgs.Account.Properties["access_token"], eventArgs.Account.Properties["refresh_token"]);
                            }
                        }


                        //App.Current.Properties["access_token"] = eventArgs.Account.Properties["access_token"].ToString();

                        ////AccountStore.Create ().Save (eventArgs.Account, "Google");

                        //// OK, if we get this far, then the user is authenticated - That's great.  
                        //// So change the App MainPage so we're at a location that only Auth users can get to.
                        //Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();


                    }
                    else
                    {
                        // Auth failed - The only way to get to this branch on Google is to hit the 'Cancel' button.
                        //App.Current.MainPage = new MainPage();

                        //App.Current.Properties["access_token"] = "";
                    }

                };

                auth.Error += (object sender, AuthenticatorErrorEventArgs e) =>
                {
                    Debug.WriteLine(e.Message);
                };



                var page = Element as LoginPage;
                if (page != null)
                {
                    var _vm = page.BindingContext as LoginPageViewModel;
                    if (_vm != null)
                    {
                        //_vm.ConnectCommand = new Command((obj) =>
                        //{
                        //    // This is what actually launches the auth web UI.
                        //    PresentViewController(auth.GetUI(), true, null);
                        //});
                    }
                }

            }

        }

    }
}
