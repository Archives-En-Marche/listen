using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.Managers;
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

        public ICommand AccountCommand
        {
            get;
            set;
        }

        public LoginPageViewModel(INavigation nav)
        {
            _nav = nav;

            //ConnectCommand = new Command(async () =>
            //{
            //});

            AccountCommand = new Command(() =>
            {
                Device.OpenUri(new Uri("https://en-marche.fr/adhesion"));
            });

        }

        public async Task AddOrUpdateTokenAsync(string token, string refreshtoken)
        {
            await UserManager.Instance.AddOrUpdateAsync(null, null, null, null, null, null, token, refreshtoken);
            //LongRunningTaskManager.Instance.StartLongRunningTask();
            await _nav.PopModalAsync();
        }
    }
}
