using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Listen.Managers;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        INavigation _nav;

        public LoginPageViewModel(INavigation nav)
        {
            _nav = nav;
        }

        public async Task AddOrUpdateTokenAsync(string token, string refreshtoken)
        {
            await UserManager.Instance.AddOrUpdateAsync("", "", "", "", token, refreshtoken);
            await _nav.PopModalAsync();
        }
    }
}
