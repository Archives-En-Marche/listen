using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Listen.VisualElements;
using Xamarin.Forms;

namespace Listen.Views
{
    public partial class InfosPage : BaseContentPage
    {
        public InfosPage(ViewModelBase vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
