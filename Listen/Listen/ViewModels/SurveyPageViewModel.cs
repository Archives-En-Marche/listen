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

namespace Listen.ViewModels
{
    public class SurveyPageViewModel : ViewModelBase
    {
        INavigation _nav;

        IList<SurveyViewCellViewModel> _surveys;
        public IList<SurveyViewCellViewModel> Surveys
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

            Surveys = new ObservableCollection<SurveyViewCellViewModel>();

            Observable.FromAsync<ObservableCollection<SurveyViewCellViewModel>>(async () =>
            {
                return await Task.Factory.StartNew(() =>
                {
                    var list = new ObservableCollection<SurveyViewCellViewModel>();
                    if (surveys?.Count > 0)
                    {
                        foreach (var s in surveys)
                        {
                            list.Add(new SurveyViewCellViewModel(s));
                        }
                    }
                    return list;
                });
            })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(list =>
            {
                var start = DateTime.Now;

                Surveys = list;

                var stop = DateTime.Now;
                Debug.WriteLine("Diff : " + (stop - start).TotalMilliseconds.ToString());
            });

            SelectedCommand = ReactiveCommand.CreateFromTask<SurveyViewCellViewModel>(async (s) =>
            {
                var survey = s as SurveyViewCellViewModel;
                if (survey != null)
                {
                    //QuestionnaireManager.Instance.InitInterview(questionnaire.Questionnaire);
                    //_nav.PushAsync(new AdressePage(new AdressePageViewModel(_nav)));
                    //_nav.PushAsync(new InterviewStep1Page(new InterviewStep1PageViewModel(_nav)));
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
    }
}
