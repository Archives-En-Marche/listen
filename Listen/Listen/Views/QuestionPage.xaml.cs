using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Listen.Contracts;
using Listen.Managers;
using Listen.ViewModels;
using Listen.VisualElements;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class QuestionPage : SoftInputPage
    {
        int nb;
        public QuestionPage(ViewModelBase vm)
        {
            BindingContext = vm;
            nb = SurveyEngineManager.Instance.QuestionNumber + 1;
            this.Title = "Question " + nb + "/"+ SurveyEngineManager.Instance.Count;
            NavigationPage.SetBackButtonTitle(this, "");
            NavigationPage.SetHasBackButton(this, true);
            InitializeComponent();

            var toolbar_arrow = new ToolbarItem()
            {
                IconImageSource = (Device.RuntimePlatform == Device.iOS ? "Images/home.png" : "home.png"),
                Text = "Accueil",
                Command = ((QuestionPageViewModel)BindingContext).BackHome
            };
            this.ToolbarItems.Add(toolbar_arrow);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var vm = BindingContext as IResetQuestion;
            if (vm != null)
            {
                vm.Reset();
            }
            if(nb > 0) { 
                SurveyEngineManager.Instance.SetPosition(nb-1);
            }
        }
    }
}
