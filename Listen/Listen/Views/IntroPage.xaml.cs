using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class IntroPage : ContentPage
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
