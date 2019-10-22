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
        public QuestionPage(ViewModelBase vm)
        {
            BindingContext = vm;
            var nb = SurveyEngineManager.Instance.QuestionNumber + 1;
            this.Title = "Question " + nb;
            NavigationPage.SetBackButtonTitle(this, "");
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();

            var toolbar_arrow = new ToolbarItem()
            {
                Icon = (Device.RuntimePlatform == Device.iOS ? "Images/path.png" : "path.png"),
                Text = "Suivante",
                Command = ((QuestionPageViewModel)BindingContext).NextQuestion
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
        }
    }
}
