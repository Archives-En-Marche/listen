using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.Managers;
using PopolLib.Services;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class UserLoginPageViewModel : ViewModelBase
    {
        INavigation _nav;

        string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                Set(() => Email, ref _email, value);
            }
        }

        string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                Set(() => Password, ref _password, value);
            }
        }

        public ICommand ValiderCommand
        {
            get;
            set;
        }

        public UserLoginPageViewModel(INavigation nav)
        {
            _nav = nav;

#if DEBUG

            Email = "paulin.laroche@edecision.fr";
            Password = "capmobile02";

            Email = "techsupport@en-marche.fr";
            Password = "enmarche123!";

            Email = "antoinetamano+13@gmail.com";
            Password = "antoinetamano+13@gmail.com";

         

            Email = "paulin.laroche@edecision.fr";
            Password = "capmobile02";

            //Email = "jul.hebrard@gmail.com";
            //Password = "123456789";

            //Email = "techsupport@en-marche.fr";
            //Password = "testapple";

#endif

            ValiderCommand = new Command(async () =>
            {
                if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
                {
                    var token = await TokenManager.Instance.GetTokenAsync(Email, Password);
                    if (token != null)
                    {
                        await UserManager.Instance.AddOrUpdateAsync(null, null, null, null, null, null, token?.AccessToken, token?.RefreshToken);
                        //var mstack = _nav.ModalStack;
                        if (_nav.ModalStack.Count > 0)
                        {
                            await _nav.PopModalAsync();
                        }
                    }
                    else
                    {
                        var dialog = DependencyService.Get<IDialogService>();
                        dialog.Show("Oups !", "Une erreur est survenue, veuillez vous reconnecter.", "OK", null);
                    }
                }
                else
                {
                    var dialog = DependencyService.Get<IDialogService>();
                    dialog.Show("Oups !", "Vos identifiants ne sont pas valides.", "OK", null);
                }
            });
        }
    }
}
