using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class SurveyPage : ContentPage
    {
        public SurveyPage(ViewModelBase vm)
        {
            BindingContext = vm;
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
        }
    }
}
