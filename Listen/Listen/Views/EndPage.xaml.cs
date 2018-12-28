using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Listen.Managers;
using Listen.Models.WebServices;
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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // -- on checke le token
            var user = await UserManager.Instance.GetUserAsync();
            if (user != null && !string.IsNullOrEmpty(user?.Token) && !string.IsNullOrEmpty(user?.RefreshToken))
            {
                var infos = await TokenWS.Instance.GetInfoAsync(user?.Token);
                if (infos != null)
                {
                    // -- on envoie que si le token est valide, sinon, cela remontera à la prochaine ouverture de l'app sur la home
                    LongRunningTaskManager.Instance.StartLongRunningTask();
                }
            }
        }
    }
}
