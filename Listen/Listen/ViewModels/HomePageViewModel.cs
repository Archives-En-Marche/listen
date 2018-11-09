using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.Helpers;
using Listen.Managers;
using Listen.Models.WebServices;
using Listen.Views;
using PopolLib.Services;
using ReactiveUI;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        INavigation _nav;

        public ICommand QuestionnaireCommand { get; set; }

        public ICommand InfosCommand { get; set; }
        public ICommand AboutCommand { get; set; }


        public HomePageViewModel(INavigation nav)
        {
            _nav = nav;

            InfosCommand = new Command(async (obj) =>
            {
                //var frame = (Frame)obj;
                //ButtonAnimationHelper.Animate(frame);
 
                //await Task.Delay(2000);
                await _nav.PushAsync(new InfosPage(new InfosPageViewModel(_nav)));
             }); 

            AboutCommand = new Command(async (obj) =>
            {
                //var btn = (Button)obj;
                //ButtonAnimationHelper.Animate(btn);

                await _nav.PushAsync(new AboutPage());
            });

            QuestionnaireCommand = new Command(async (obj) =>
            {
                //var hud = DependencyService.Get<IProgressHUD>();
                //hud.Show("Chargement ...");
                //var surveys = await SurveyManager.Instance.GetSurveysAsync();

                //hud.Dismiss();

                //if (surveys?.Count > 0)
                {
                    await _nav.PushAsync(new SurveyPage(new SurveyPageViewModel(_nav, null)));
                }
                //else
                {
                    //var dialog = DependencyService.Get<IDialogService>();
                    //dialog.Show("Erreur", "Aucun formulaires disponibles.", "Oui", (_obj) => { });
                }
            });
        }
    }
}
