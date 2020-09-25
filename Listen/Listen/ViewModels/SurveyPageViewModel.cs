using System;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
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
using System.Reactive;

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

        public ReactiveCommand<IFastViewCell, Unit> SelectedCommand { get; set; }

        public SurveyPageViewModel(INavigation nav, IList<Survey> surveys)
        {
            _nav = nav;

            var user = UserManager.Instance.GetUser();

            Surveys = new ObservableCollection<IFastViewCell>();
            if (Device.RuntimePlatform == Device.Android)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var list = new ObservableCollection<IFastViewCell>();
                    list.Add(new NoSurveyViewCellViewModel() { Name = "Bonjour " + user?.FirstName + " !" });
                    list.Add(new UpdateSurveyViewCellViewModel()
                    {
                        ActualiserCommand = new Command(async () =>
                        {
                            var _list = await ServerManager.Instance.GetSurveysAsync();
                            if (_list == null)
                            {
                                Debug.WriteLine("[TOKEN_ERROR]");
                                // -- On delete le user en base
                                await UserManager.Instance.DeleteUserAsync();
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await _nav.PopToRootAsync();
                                });
                            }
                            else
                            {
                                UpdateUI(_list);
                            }
                        })
                    });
                    Surveys = list;
                });
            }
            else
            {
                Surveys.Add(new NoSurveyViewCellViewModel() { Name = "Bonjour " + user?.FirstName + " !" });
                Surveys.Add(new UpdateSurveyViewCellViewModel()
                {
                    ActualiserCommand = new Command(async () =>
                    {
                        var list = await ServerManager.Instance.GetSurveysAsync();
                        if (list == null)
                        {
                            Debug.WriteLine("[TOKEN_ERROR]");
                            // -- On delete le user en base
                            await UserManager.Instance.DeleteUserAsync();
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                await _nav.PopToRootAsync();
                            });
                        }
                        else
                        {
                            UpdateUI(list);
                        }
                    })
                });
            }

            Task.Factory.StartNew(async () =>
            {
                var list = await ServerManager.Instance.GetSurveysAsync();
                if (list == null)
                {
                    Debug.WriteLine("[TOKEN ERROR]");
                    // -- On delete le user en base
                    await UserManager.Instance.DeleteUserAsync();
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await _nav.PopToRootAsync();
                    });
                }
                else
                {
                    UpdateUI(list);
                }
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
                    if (s is UpdateSurveyViewCellViewModel vm)
                    {
                        var list = await ServerManager.Instance.GetSurveysAsync();
                        if (list == null)
                        {
                            Debug.WriteLine("[TOKEN ERROR]");
                            // -- On delete le user en base
                            await UserManager.Instance.DeleteUserAsync();
                            Device.BeginInvokeOnMainThread(async () =>
                                                        {
                                                            await _nav.PopToRootAsync();
                                                        });

                        }
                    }
                }

            });
        }

        private void UpdateUI(object obj)
        {
            Observable.FromAsync<ObservableCollection<IFastViewCell>>(async () =>
            {
                try
                {
                    await Task.Delay(500);
                    var user = await UserManager.Instance.GetUserAsync();
                    if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.ZipCode))
                    {
                        await ServerManager.Instance.GetUserInfosAsync();
                        user = await UserManager.Instance.GetUserAsync();
                    }
                    var surveys = (IList<Survey>)obj;
                    var list = new ObservableCollection<IFastViewCell>();
                    list.Add(new UserWelcomeViewCellViewModel(user.FirstName, user.ZipCode.Substring(0, 2), surveys?.Count > 0));
                    //var surveys = await SurveyManager.Instance.GetSurveysAsync();
                    if (surveys?.Count > 0)
                    {
                        foreach (var s in surveys)
                        {
                            list.Add(new SurveyViewCellViewModel(s));
                        }
                    }
                    return list;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return null;
                }
            })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(list =>
            {
                try
                {
                    if (list.Count > 0)
                    {
                        var start = DateTime.Now;

                        Surveys = list;

                        var stop = DateTime.Now;
                        Debug.WriteLine("Diff : " + (stop - start).TotalMilliseconds.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            });
        }
    }
}
