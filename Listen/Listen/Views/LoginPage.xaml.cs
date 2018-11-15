using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(ViewModelBase vm)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "");
            this.Title = "";
            BindingContext = vm;
            InitializeComponent();
        }


        public void Handle_Tapped(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new SubscribePage());
        }
    }
}
