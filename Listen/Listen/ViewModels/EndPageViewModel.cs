using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.Helpers;
using Listen.Managers;
using Listen.Models.RealmObjects;
using Listen.Views;
using ReactiveUI;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class EndPageViewModel : ViewModelBase
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

        public ICommand NewCommand { get; set; }

        public ICommand AllCommand { get; set; }

        public EndPageViewModel(INavigation navigation)
        {
            _nav = navigation;

            Observable.FromAsync<User>(async () =>
            {
                var user = await UserManager.Instance.GetUserAsync();
                if (user != null && string.IsNullOrEmpty(user?.FirstName))
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

               Fullname = "Bravo " + user.FirstName + " !";

               var stop = DateTime.Now;
               Debug.WriteLine("Diff : " + (stop - start).TotalMilliseconds.ToString());
           });

            NewCommand = new Command(async (obj) =>
            {
                var frame = (Frame)obj;
                ButtonAnimationHelper.Animate(frame);

                for (int i = _nav.NavigationStack.Count - 2; i >= 0; i--)
                {
                    var p = _nav.NavigationStack[i];
                    if (!(p is SurveyPage) && !(p is HomePage) && !(p is IntroPage))
                    {
                        _nav.RemovePage(p);
                    }
                }
                SurveyEngineManager.Instance.InitCurrentSurvey();
                await _nav.PopAsync();
            });

            AllCommand = new Command(async (obj) =>
            {
                var frame = (Frame)obj;
                ButtonAnimationHelper.Animate(frame);

                for (int i = _nav.NavigationStack.Count - 2; i >= 0; i--)
                {
                    var p = _nav.NavigationStack[i];
                    if (!(p is SurveyPage) && !(p is HomePage))
                    {
                        _nav.RemovePage(p);
                    }
                }
                await _nav.PopAsync();
            });
        }
    }
}
