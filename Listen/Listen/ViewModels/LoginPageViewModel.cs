using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.Managers;
using Listen.Views;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Listen.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        INavigation _nav;

        public ICommand RegisterCommand
        {
            get;
            set;
        }

        public ICommand ConnectCommand
        {
            get;
            set;
        }

        public LoginPageViewModel(INavigation nav)
        {
            _nav = nav;

            ConnectCommand = new Command(async (obj) =>
            {
                await _nav.PushAsync(new UserLoginPage(new UserLoginPageViewModel(_nav)));
            });

            RegisterCommand = new Command(async () =>
            {
                var url = "https://www.en-marche.fr/adhesion";

#if DEBUG
                url = "https://staging.en-marche.fr/adhesion";
#endif

                await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            });
        }

        public async Task AddOrUpdateTokenAsync(string token, string refreshtoken)
        {
            await UserManager.Instance.AddOrUpdateAsync(null, null, null, null, null, null, token, refreshtoken);

            var mstack = _nav.ModalStack;

            foreach (var p in mstack)
            {
                await _nav.PopModalAsync();
            }
        }
    }
}
