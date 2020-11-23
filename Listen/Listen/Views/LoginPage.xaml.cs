using System;
using GalaSoft.MvvmLight;
using Listen.ViewModels;
using Listen.VisualElements;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class LoginPage : BaseContentPage
    {
        public LoginPage(ViewModelBase vm)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "");
            this.Title = "";
            BindingContext = vm;
            InitializeComponent();
        }


        //public void Handle_Tapped(object sender, EventArgs e)
        //{
        //    this.Navigation.PushAsync(new SubscribePage());
        //}

        public void Handle_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new UserLoginPage(new UserLoginPageViewModel(this.Navigation)));
        }
    }
}
