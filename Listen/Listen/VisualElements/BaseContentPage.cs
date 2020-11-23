using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Listen.VisualElements
{
    public class BaseContentPage : ContentPage
    {
        public BaseContentPage()
        {
            On<iOS>().SetUseSafeArea(false);
            BackgroundColor = (Color) Xamarin.Forms.Application.Current.Resources["paleGrey"];
        }
    }
}
