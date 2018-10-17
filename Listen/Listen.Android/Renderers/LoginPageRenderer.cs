using System;
using Android.App;
using Android.Content;
using Listen.Droid.Renderers;
using Listen.OAuth;
using Listen.ViewModels;
using Listen.Views;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using OAuth2Authenticator = Listen.OAuth.OAuth2Authenticator;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]
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

                var auth = new OAuth2Authenticator(
                    clientId: "2b494496-4eae-4946-9ae1-efa3f593595c",
                    clientSecret: "vcyrJys1sdvaTCXN0BFfOuw2A8KxdA9QkYDMErViM68=",
                    scope: null,
                    authorizeUrl: new Uri("https://staging.en-marche.fr/oauth/v2/auth"),
                    redirectUrl: new Uri("https://staging.en-marche.fr"),
                    accessTokenUrl: new Uri("https://staging.en-marche.fr/oauth/v2/token"))
                {
                    ShowErrors = false
                };

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

                //auth.AllowCancel = false;
                activity.StartActivity(auth.GetUI(activity));
                done = true;
            }
        }
    }
}
