using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        INavigation _nav;

        public ICommand QuestionnaireCommand { get; set; }
 
        public ICommand InfosCommand { get; set; }


        public HomePageViewModel(INavigation nav)
        {
            _nav = nav;

            InfosCommand = new Command(async () =>
            {
                await Task.Delay(2000);
                //await _nav.PushAsync(new ParametresPage(new ParametresPageViewModel(_nav)));
            });

            QuestionnaireCommand = new Command(async () =>
            {
                await Task.Delay(2000);

                //var questionnaires = await QuestionnaireManager.Instance.GetAllAsync();
                //if (questionnaires.Count > 0)
                //{
                //    await _nav.PushAsync(new QuestionairesPage(new QuestionnairesPageViewModel(_nav)));
                //}
                //else
                //{
                //    var dialog = DependencyService.Get<IDialogService>();
                //    dialog.Show("Erreur", "Une erreur s'est produite lors du chargement de votre application.\nVeuillez relancer votre application.", "Oui", (_obj) => { });
                //}

            });

         }
    }
}
