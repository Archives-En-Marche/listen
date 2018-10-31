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
                    if (p is RgpdPage)
                    {
                        _nav.RemovePage(p);
                    }
                    if ((i - 1) > 0)
                    {
                        var b = _nav.NavigationStack[i - 1] as SurveyPage;
                        if (p is QuestionPage && b == null)
                        {
                            _nav.RemovePage(p);
                            // -- On reinitialise la réponse
                            //((InterviewedStep2Page)p).ReInitInterview(new Reponse() { InterviewArrondissement = _interviewArrondissement, InterviewStationMetro = _interviewStationMetro });
                        }
                        else if (b != null)
                        {
                            // -- On reinitialise les survey engine
                            SurveyEngineManager.Instance.InitCurrentSurvey();
                        }
                    }
                }
                await _nav.PopAsync();
            });

            AllCommand = new Command(async (obj) => 
            {
                var frame = (Frame)obj;
                ButtonAnimationHelper.Animate(frame);

                for (int i = _nav.NavigationStack.Count - 2; i >= 0; i--)
                {
                    var p = _nav.NavigationStack[i];
                    if (p is QuestionPage || p is RgpdPage)
                    {
                        _nav.RemovePage(p);
                    }
                    if (p is SurveyPage)
                    {
                        // -- On reinitialise la réponse
                        //((InterviewedStep2Page)p).ReInitInterview(new Reponse() { InterviewArrondissement = _interviewArrondissement, InterviewStationMetro = _interviewStationMetro });
                    }
                }
                await _nav.PopAsync();
            });
        }
    }
}
