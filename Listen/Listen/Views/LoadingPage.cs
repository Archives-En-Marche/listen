using System;
using GalaSoft.MvvmLight;
using Listen.VisualElements;
using Xamarin.Forms;

namespace Listen.Views
{
    public class LoadingPage : BaseContentPage
    {
        public LoadingPage(ViewModelBase vm)
        {
            BindingContext = vm;
        }
    }
}

