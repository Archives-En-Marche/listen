using System;
using System.Linq;
using Listen.iOS.Renderers;
using Listen.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(CustomPageRenderer))]
namespace Listen.iOS.Renderers
{
    public class CustomPageRenderer : PageRenderer
    {

        public override void ViewWillAppear(bool animated)
        {
            base.ViewDidLoad();
            var page = Element as ContentPage;
            if (page == null) return;

            if (page is HomePage)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, false);
                    GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, false);
                    GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
                });
            }
        }

        UIViewController GetCurrentViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;

            if (vc is UINavigationController navController)
                vc = navController.ViewControllers.Last();

            return vc;
        }
    }
}
