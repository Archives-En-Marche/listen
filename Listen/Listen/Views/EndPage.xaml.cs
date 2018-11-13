using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Listen.Managers;
using Listen.ViewModels;
using PopolLib.Extensions;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class EndPage : ContentPage
    {
        public EndPage(ViewModelBase vm)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "");
            BindingContext = vm;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LongRunningTaskManager.Instance.StartLongRunningTask();
        }
    }
}
