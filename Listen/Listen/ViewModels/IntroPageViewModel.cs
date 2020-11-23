using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.ViewModels;
using Listen.Managers;
using Listen.Views;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class IntroPageViewModel : ViewModelBase
    {
        INavigation _nav;

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                Set(() => Name, ref _name, value);
            }
        }

        public ICommand ContinueCommand { get; set; }

        public IntroPageViewModel(INavigation navigation)
        {
            _nav = navigation;

            var user = UserManager.Instance.GetUser();

            Name = user.FirstName;

            ContinueCommand = new Command(async () =>
            {
                await _nav.PushAsync(new QuestionPage(new QuestionPageViewModel(_nav, true)));
            });
        }
    }
}
