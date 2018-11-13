using System;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Listen.ViewModels.ViewCell;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

using Listen.Managers;
using ReactiveUI;
using System.Diagnostics;
using System.Threading.Tasks;
using Listen.Models.RealmObjects;
using Listen.Views;
using PopolLib.Contracts.FastListView;

namespace Listen.ViewModels
{
    public class SurveyPageViewModel : ViewModelBase
    {
        INavigation _nav;

        IList<IFastViewCell> _surveys;
        public IList<IFastViewCell> Surveys
        {
            get
            {
                return _surveys;
            }
            set
            {
                Set(() => Surveys, ref _surveys, value);
            }
        }

        public ReactiveCommand SelectedCommand { get; set; }

        public SurveyPageViewModel(INavigation nav, IList<Survey> surveys)
        {
            _nav = nav;

            MessagingCenter.Subscribe<SurveyManager, IList<Survey>>(this, "UpdateUI", (sender, arg) =>
            {
                UpdateUI(arg);
                if (Device.RuntimePlatform == Device.Android)
                {
                    MessagingCenter.Unsubscribe<SurveyManager, IList<Survey>>(this, "UpdateUI");
                }
            });

            var user = UserManager.Instance.GetUser();

            Surveys = new ObservableCollection<IFastViewCell>();
            if (Device.RuntimePlatform == Device.Android)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var list = new ObservableCollection<IFastViewCell>();
                    list.Add(new NoSurveyViewCellViewModel() { Name = "Bonjour " + user.FirstName + " !" });
                    list.Add(new UpdateSurveyViewCellViewModel() 
                    { 
                        ActualiserCommand = new Command(async () => await SurveyManager.Instance.GetSurveysAsync()) 
                    });
                    Surveys = list;
                });
            }
            else
            {
                Surveys.Add(new NoSurveyViewCellViewModel() { Name = "Bonjour " + user.FirstName + " !" });
                Surveys.Add(new UpdateSurveyViewCellViewModel() 
                { 
                    ActualiserCommand = new Command(async () => await SurveyManager.Instance.GetSurveysAsync()) 
                });
            }

            Task.Factory.StartNew(async () =>
            {
                await SurveyManager.Instance.GetSurveysAsync();
            });

            SelectedCommand = ReactiveCommand.CreateFromTask<IFastViewCell>(async (s) =>
            {
                {
                    if (s is SurveyViewCellViewModel vm)
                    {
                        SurveyEngineManager.Instance.Init(vm.Survey);
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await _nav.PushAsync(new IntroPage(new IntroPageViewModel(_nav)));
                        });
                    }
                }
                {
                    //vm.ActualiserCommand.Execute(null);
                    if (s is UpdateSurveyViewCellViewModel vm)
                    {
                        await SurveyManager.Instance.GetSurveysAsync();
                    }
                }

            });
         }

        private void UpdateUI(object obj)
        {
            Observable.FromAsync<ObservableCollection<IFastViewCell>>(async () =>
            {
                await Task.Delay(500);
                var user = await UserManager.Instance.GetUserAsync();
                if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.ZipCode))
                {
                    await ServerManager.Instance.GetUserInfosAsync(user.Token);
                    user = await UserManager.Instance.GetUserAsync();
                }
                var list = new ObservableCollection<IFastViewCell>();
                list.Add(new UserWelcomeViewCellViewModel(user.FirstName, user.ZipCode.Substring(0, 2)));
                //var surveys = await SurveyManager.Instance.GetSurveysAsync();
                var surveys = (IList<Survey>)obj;
                if (surveys?.Count > 0)
                {
                    foreach (var s in surveys)
                    {
                        list.Add(new SurveyViewCellViewModel(s));
                    }
                }
                return list;
            })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(list =>
            {
                var start = DateTime.Now;

                Surveys = list;

                var stop = DateTime.Now;
                Debug.WriteLine("Diff : " + (stop - start).TotalMilliseconds.ToString());
            });
        }
    }
}
