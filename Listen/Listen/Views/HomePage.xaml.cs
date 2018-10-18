using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage(ViewModelBase vm)
        {
            this.Title = "Accueil";
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "");
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
