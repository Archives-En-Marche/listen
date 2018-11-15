using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.Managers;
using Listen.Views;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        INavigation _nav;

        ICommand _connectCommand;
        public ICommand ConnectCommand
        {
            get { return _connectCommand; }
            set { Set(() => ConnectCommand, ref _connectCommand, value); }
        }

        public LoginPageViewModel(INavigation nav)
        {
            _nav = nav;
        }

        public async Task AddOrUpdateTokenAsync(string token, string refreshtoken)
        {
            await UserManager.Instance.AddOrUpdateAsync(null, null, null, null, null, null, token, refreshtoken);
            await _nav.PopModalAsync();
        }
    }
}
