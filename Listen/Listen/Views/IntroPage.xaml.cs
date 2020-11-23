using GalaSoft.MvvmLight;
using Listen.Managers;
using Listen.VisualElements;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class IntroPage : BaseContentPage
    {
        public IntroPage(ViewModelBase vm)
        {
            Title = "Intro";
            NavigationPage.SetBackButtonTitle(this, "");
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
