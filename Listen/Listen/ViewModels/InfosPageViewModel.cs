using System;
using System.Diagnostics;
using System.Reactive.Linq;
using GalaSoft.MvvmLight;
using Listen.Managers;
using Listen.Models.RealmObjects;
using ReactiveUI;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class InfosPageViewModel : ViewModelBase
    {
        INavigation _nav;

        string _fullname;
        public string Fullname
        {
            get
            {
                return _fullname;
            }
            set
            {
                Set(() => Fullname, ref _fullname, value);
            }
        }

        string _departement;
        public string Departement
        {
            get
            {
                return _departement;
            }
            set
            {
                Set(() => Departement, ref _departement, value);
            }
        }

        public ReactiveCommand LogoutCommand { get; set; }

        public InfosPageViewModel(INavigation navigation)
        {
            _nav = navigation;

            Observable.FromAsync<User>(async () =>
            {
                var user = await UserManager.Instance.GetUserAsync();
                if (user != null && string.IsNullOrEmpty(user?.FirstName) || string.IsNullOrEmpty(user?.ZipCode))
                {
                    await ServerManager.Instance.GetUserInfosAsync(user.Token);
                    user = await UserManager.Instance.GetUserAsync();
                }
                return user;
            })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(user =>
            {
                var start = DateTime.Now;

                Fullname = "Bonjour " + user.FirstName + " !";
                Departement = "Vous habitez dans le département " + user.ZipCode.Substring(0, 2) + ".";

                var stop = DateTime.Now;
                Debug.WriteLine("Diff : " + (stop - start).TotalMilliseconds.ToString());
            });

            LogoutCommand = ReactiveCommand.CreateFromTask<InfosPageViewModel>(async (s) =>
            {
                // -- On delete le user en base
                await UserManager.Instance.DeleteUserAsync();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await _nav.PopToRootAsync();
                });
            });
        }
    }
}
