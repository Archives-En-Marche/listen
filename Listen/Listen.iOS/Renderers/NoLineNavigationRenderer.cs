using System;
using Listen.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NoLineNavigationRenderer))]
namespace Listen.iOS.Renderers
{
    public class NoLineNavigationRenderer : NavigationRenderer
    {

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // remove lower border and shadow of the navigation bar
            NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
            NavigationBar.ShadowImage = new UIImage();
        }
    }
}