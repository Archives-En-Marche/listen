using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class UserLoginPage : ContentPage
    {
        public UserLoginPage()
        {
        }
            
        public UserLoginPage(ViewModelBase vm)
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "");
            this.Title = "";
            BindingContext = vm; InitializeComponent();
        }
    }
}
