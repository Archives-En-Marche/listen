using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.Managers;
using Listen.Views;
using Listen.VisualElements;
using PopolLib.Services;
using Xamarin.Essentials;
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

        bool _hidePassword;
        public bool HidePassword
        {
            get => _hidePassword;
            set
            {
                Set(ref _hidePassword, value);
                HidePasswordMessage = HidePassword ? "Afficher le mot de passe" : "Masquer le mot de passe";
            }
        }

        string _hidePasswordMessage;
        public string HidePasswordMessage
        {
            get => _hidePasswordMessage;
            set
            {
                Set(ref _hidePasswordMessage, value);
            }
        }

        public ICommand ValiderCommand
        {
            get;
            set;
        }

        public ICommand PasswordLostCommand
        {
            get;
            set;
        }

        public ICommand ShowPasswordCommand
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
                        Application.Current.MainPage = new InternalNavigationPage(new HomePage(_nav, new HomePageViewModel(_nav)));
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

            PasswordLostCommand = new Command(async () =>
            {
                var url = "https://www.en-marche.fr/mot-de-passe-oublie";

#if DEBUG
                url = "https://staging.en-marche.fr/mot-de-passe-oublie";
#endif

                await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            });

            HidePassword = true;
            ShowPasswordCommand = new Command(() =>
            {
                HidePassword = !HidePassword;
            });
        }
    }
}
