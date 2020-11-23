using GalaSoft.MvvmLight;
using Listen.VisualElements;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class IntroPage : BaseContentPage
    {
        public IntroPage(ViewModelBase vm)
        {
            Title = "";
            NavigationPage.SetBackButtonTitle(this, "");
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
