using GalaSoft.MvvmLight;
using Listen.ViewModels;
using Listen.VisualElements;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class MoreInfosPage : BaseContentPage
    {
        public MoreInfosPage(ViewModelBase vm)
        {
            BindingContext = vm;
            NavigationPage.SetBackButtonTitle(this, "");
            NavigationPage.SetHasBackButton(this, true);
            this.Title = "Plus d'infos";
            InitializeComponent();

            var toolbar_arrow = new ToolbarItem()
            {
                IconImageSource = (Device.RuntimePlatform == Device.iOS ? "Images/home.png" : "home.png"),
                Text = "Accueil",
                Command = ((MoreInfosPageViewModel)BindingContext).BackHome
            };
            this.ToolbarItems.Add(toolbar_arrow);
        }
    }
}
