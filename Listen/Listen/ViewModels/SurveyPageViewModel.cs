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

            Surveys = new ObservableCollection<IFastViewCell>();
            if (Device.RuntimePlatform == Device.Android)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var list = new ObservableCollection<IFastViewCell>();
                    list.Add(new NoSurveyViewCellViewModel());
                    Surveys = list;
                });
            }
            else
            {
                Surveys.Add(new NoSurveyViewCellViewModel());
            }
            Task.Factory.StartNew(async () =>
            {
                await SurveyManager.Instance.GetSurveysAsync();
            });


            //MessagingCenter.Subscribe<ServerManager>(this, "UpdateUI", UpdateUI);

            //Observable.FromAsync<ObservableCollection<IFastViewCell>>(async () =>
            //{
            //    return await Task.Factory.StartNew(() =>
            //    {
            //        var list = new ObservableCollection<IFastViewCell>();
            //        //list.Add(new UserWelcomeViewCellViewModel("Paulin", "75"));
            //        //if (surveys?.Count > 0)
            //        //{
            //        //    list.Add(new UserWelcomeViewCellViewModel("Paulin", "75"));
            //        //    foreach (var s in surveys)
            //        //    {
            //        //        list.Add(new SurveyViewCellViewModel(s));
            //        //    }
            //        //}
            //        //else 
            //        {
            //            list.Add(new NoSurveyViewCellViewModel());

            //        }
            //        return list;
            //    });
            //})
            //.ObserveOn(RxApp.MainThreadScheduler)
            //.Subscribe(list =>
            //{
            //    var start = DateTime.Now;

            //    Surveys = list;

            //    var stop = DateTime.Now;
            //    Debug.WriteLine("Diff : " + (stop - start).TotalMilliseconds.ToString());
            //});

            SelectedCommand = ReactiveCommand.CreateFromTask<IFastViewCell>(async (s) =>
            {
                var vm = s as SurveyViewCellViewModel;
                if (vm != null)
                {
                    SurveyEngineManager.Instance.Init(vm.Survey);
                    //QuestionnaireManager.Instance.InitInterview(questionnaire.Questionnaire);
                    //_nav.PushAsync(new AdressePage(new AdressePageViewModel(_nav)));
                    //_nav.PushAsync(new InterviewStep1Page(new InterviewStep1PageViewModel(_nav)));
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await _nav.PushAsync(new QuestionPage(new QuestionPageViewModel(_nav)));
                    });
                }
            });
            //    new Command((q) =>
            //    {
            //        var questionnaire = q as SurveyViewCellViewModel;
            //        if (questionnaire != null)
            //        {
            //            //QuestionnaireManager.Instance.InitInterview(questionnaire.Questionnaire);
            //            //_nav.PushAsync(new AdressePage(new AdressePageViewModel(_nav)));
            //            //_nav.PushAsync(new InterviewStep1Page(new InterviewStep1PageViewModel(_nav)));
            //        }
            //    });
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
