using System;
using Android.App;
using Android.Content;
using Listen.Droid.Renderers;
using Listen.Helpers;
using Listen.OAuth;
using Listen.ViewModels;
using Listen.Views;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OAuth2Authenticator = Listen.OAuth.OAuth2Authenticator;

//[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]
namespace Listen.Droid.Renderers
{
    public class LoginPageRenderer : PageRenderer
    {
        bool done = false;
        Context ctxt;

        public LoginPageRenderer(Context context)
            : base(context)
        {
            ctxt = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (!done)
            {

                // this is a ViewGroup - so should be able to load an AXML file and FindView<>
                var activity = ctxt as Activity;

#if DEBUG
                var auth = new OAuth2Authenticator(
                    clientId: "2b494496-4eae-4946-9ae1-efa3f593595c",
                    clientSecret: "",
                    scope: "jecoute_surveys",
                    authorizeUrl: new Uri("https://staging.en-marche.fr/oauth/v2/auth"),
                    redirectUrl: new Uri("https://staging.en-marche.fr"),
                    accessTokenUrl: new Uri("https://staging.en-marche.fr/oauth/v2/token"))
                {
                    ShowErrors = false,
                    AllowCancel = false,
                    Title = ""
                };
#else
                var auth = new OAuth2Authenticator(
                    "33f63935-0793-41b9-89a8-1d9bb74e5fe5",
                    "",
                    "jecoute_surveys",
                    new Uri("https://www.en-marche.fr/oauth/v2/auth"),
                    new Uri("https://en-marche.fr"),
                    new Uri("https://www.en-marche.fr/oauth/v2/token"))
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
                        if (Element is LoginPage lp)
                        {
                            if (lp.BindingContext is LoginPageViewModel vm)
                            {
                                await vm.AddOrUpdateTokenAsync(eventArgs.Account.Properties["access_token"], eventArgs.Account.Properties["refresh_token"]);
                            }
                        }

                        //App.Current.Properties["access_token"] = eventArgs.Account.Properties["access_token"].ToString();

                        //AccountStore.Create (this).Save (eventArgs.Account, "Google");
                    }
                    else
                    {
                        // Auth failed - The only way to get to this branch on Google is to hit the 'Cancel' button.
                        //App.Current.MainPage = new LoginPage();
                        //App.Current.Properties["access_token"] = "";
                    }
                };

                auth.Error += (object sender, AuthenticatorErrorEventArgs _e) =>
                {
                    Console.WriteLine(_e.Message);
                };

                //var page = Element as LoginPage;
                //if (page != null)
                //{
                //    var _vm = page.BindingContext as LoginPageViewModel;
                //    if (_vm != null)
                //    {
                //        _vm.ConnectCommand = new Command((obj) =>
                //        {
                //            // This is what actually launches the auth web UI.
                //            activity.StartActivity(auth.GetUI(activity));
                //            done = true;
                //        });
                //    }
                //}

            }
        }
    }
}
