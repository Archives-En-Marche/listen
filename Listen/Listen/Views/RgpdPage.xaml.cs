using GalaSoft.MvvmLight;
using Listen.ViewModels;
using Listen.VisualElements;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class RgpdPage : BaseContentPage
    {
        public RgpdPage(ViewModelBase vm)
        {
            NavigationPage.SetBackButtonTitle(this, "");
            NavigationPage.SetHasBackButton(this, true);
            this.Title = "Demande de contact";
            BindingContext = vm;
            InitializeComponent();

            var toolbar_arrow = new ToolbarItem()
            {
                IconImageSource = (Device.RuntimePlatform == Device.iOS ? "Images/home.png" : "home.png"),
                Text = "Accueil",
                Command = ((RgpdPageViewModel)BindingContext).BackHome
            };
            this.ToolbarItems.Add(toolbar_arrow);
        }
    }
}
