using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class InfosPage : ContentPage
    {
        public InfosPage(ViewModelBase vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
