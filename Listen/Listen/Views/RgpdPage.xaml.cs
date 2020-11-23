using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class RgpdPage : ContentPage
    {
        public RgpdPage(ViewModelBase vm)
        {
            NavigationPage.SetHasBackButton(this, false);
            this.Title = "Demande de contact";
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
