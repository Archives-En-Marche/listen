using GalaSoft.MvvmLight;
using Listen.VisualElements;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class SurveyPage : BaseContentPage
    {
        public SurveyPage(ViewModelBase vm)
        {
            BindingContext = vm;
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
        }
    }
}
