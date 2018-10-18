using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class SurveyPage : ContentPage
    {
        public SurveyPage(ViewModelBase vm)
        {
            this.Title = "Questionnaires";
            BindingContext = vm;
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
        }
    }
}
