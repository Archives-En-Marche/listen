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
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
