using System;
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

            if (Element is IntroPage)
            {
                this.NavigationController.TopViewController.NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(
                    UIImage.FromFile("Images/home.png"), UIBarButtonItemStyle.Plain, (sender, args) =>
                    {
                        NavigationController.PopViewController(true);
                    }), true);
            }
        }
    }
}
