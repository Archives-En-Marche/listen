using GalaSoft.MvvmLight;
using Listen.VisualElements;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class RgpdPage : BaseContentPage
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
