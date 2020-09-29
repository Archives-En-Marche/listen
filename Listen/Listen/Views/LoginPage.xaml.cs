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
    }
}
