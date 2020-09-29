using GalaSoft.MvvmLight;
using Listen.VisualElements;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class UserLoginPage : BaseContentPage
    {
        public UserLoginPage()
        {
        }
            
        public UserLoginPage(ViewModelBase vm)
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "");
            this.Title = "";
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
