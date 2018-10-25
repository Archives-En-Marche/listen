using System;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class LoadingPageViewModel
    {
        INavigation _nav;

        public LoadingPageViewModel(INavigation navigation)
        {
            _nav = navigation;
        }
    }
}
