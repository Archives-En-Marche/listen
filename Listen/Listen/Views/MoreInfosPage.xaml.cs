using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class MoreInfosPage : ContentPage
    {
        public MoreInfosPage(ViewModelBase vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
