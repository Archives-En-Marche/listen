using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Listen.Managers;
using Listen.ViewModels;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class QuestionPage : ContentPage
    {
        public QuestionPage(ViewModelBase vm)
        {
            BindingContext = vm;
            this.Title = "Question " + SurveyEngineManager.Instance.QuestionNumber;
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
    }
}
