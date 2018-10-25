using System;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.Views
{
    public class LoadingPage : ContentPage
    {
        public LoadingPage(ViewModelBase vm)
        {
            BindingContext = vm;
        }
    }
}

