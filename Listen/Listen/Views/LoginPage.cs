using System;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.Views
{
    public class LoginPage : ContentPage
    {
        public LoginPage(ViewModelBase vm)
        {
            BindingContext = vm;
        }
    }
}

