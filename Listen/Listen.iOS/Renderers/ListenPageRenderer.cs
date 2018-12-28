using System;
using System.Linq;
using Listen.iOS.Renderers;
using Listen.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(ListenPageRenderer))]
namespace Listen.iOS.Renderers
{
    public class ListenPageRenderer : PageRenderer
    {
        //UIBarButtonItem[] list = null;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (NavigationController == null)
                return;

            Device.BeginInvokeOnMainThread(() =>
            {
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, false);
                GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
                UINavigationBar.Appearance.BarTintColor = Color.FromHex("#eff9ff").ToUIColor();
                UINavigationBar.Appearance.TintColor = Color.FromHex("#174163").ToUIColor();

                UINavigationBar.Appearance.ShadowImage = new UIImage();
                UINavigationBar.Appearance.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            });

            if (Element is IntroPage)
            {
                this.NavigationController.TopViewController.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(
                    UIImage.FromFile("Images/home.png"), UIBarButtonItemStyle.Plain, (sender, args) =>
                    {
                        NavigationController.PopViewController(true);
                    }), true);
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
