using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.ViewModels.Tags;
using Listen.Views;
using PopolLib.Services;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class RgpdPageViewModel : ViewModelBase
    {
        INavigation _nav;

        TagViewModel _q1SelectedYesNo;
        public TagViewModel Q1SelectedYesNo
        {
            get
            {
                return _q1SelectedYesNo;
            }
            set
            {
                Set(() => Q1SelectedYesNo, ref _q1SelectedYesNo, value);
            }
        }

        IList<TagViewModel> _q1YesNoItems;
        public IList<TagViewModel> Q1YesNoItems { get { return _q1YesNoItems; } set { Set(() => Q1YesNoItems, ref _q1YesNoItems, value); } }

        TagViewModel _q2SelectedYesNo;
        public TagViewModel Q2SelectedYesNo
        {
            get
            {
                return _q2SelectedYesNo;
            }
            set
            {
                Set(() => Q2SelectedYesNo, ref _q2SelectedYesNo, value);
            }
        }

        IList<TagViewModel> _q2YesNoItems;
        public IList<TagViewModel> Q2YesNoItems { get { return _q2YesNoItems; } set { Set(() => Q2YesNoItems, ref _q2YesNoItems, value); } }

        public ICommand Q1YesNoCommand { get; set; }
        public ICommand Q2YesNoCommand { get; set; }

        string _nom;
        public string Nom
        {
            get
            {
                return _nom;
            }
            set
            {
                Set(() => Nom, ref _nom, value);
            }
        }

        string _prenom;
        public string Prenom
        {
            get
            {
                return _prenom;
            }
            set
            {
                Set(() => Prenom, ref _prenom, value);
            }
        }

        string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                Set(() => Email, ref _email, value);
            }
        }

        string _telephone;
        public string Telephone
        {
            get
            {
                return _telephone;
            }
            set
            {
                Set(() => Telephone, ref _telephone, value);
            }
        }

        public ICommand ValiderCommand
        {
            get;
            set;
        }

        //public RgpdPageViewModel(INavigation nav, Reponse reponse)
        public RgpdPageViewModel(INavigation nav)
        {
            _nav = nav;

            Q1YesNoItems = new ObservableCollection<TagViewModel>();
            {
                var f = new TagViewModel
                {
                    Text = "Oui",
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                Q1YesNoItems.Add(f);
                //Q1SelectedYesNo = f;
            }
            {
                var f = new TagViewModel
                {
                    Text = "Non",
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                Q1YesNoItems.Add(f);
            }

            Q2YesNoItems = new ObservableCollection<TagViewModel>();
            {
                var f = new TagViewModel
                {
                    Text = "Oui",
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                Q2YesNoItems.Add(f);
                //Q2SelectedYesNo = f;
            }
            {
                var f = new TagViewModel
                {
                    Text = "Non",
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                Q2YesNoItems.Add(f);
            }

            Q1YesNoCommand = new Command(Q1YesNoSelected);
            Q2YesNoCommand = new Command(Q2YesNoSelected);

            ValiderCommand = new Command(async () =>
            {
                var emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
                if (string.IsNullOrEmpty(Email))
                {
                    Email = "";
                }

                var ok = (Regex.IsMatch(Email.Trim(), emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
    
                if (Q1SelectedYesNo != null && Q2SelectedYesNo != null && ok)
                {
                    await _nav.PopToRootAsync();
                }
                else
                {
                    var dialog = DependencyService.Get<IDialogService>();
                    dialog.Show("Erreur", "Merci de :\n - Renseigner un email valide,\n - De répondre aux 2 questions.", "OUI", null);
                }


                //reponse.Nom = Nom;
                //reponse.Prenom = Prenom;
                //reponse.Email = Email;
                //reponse.Telephone = Telephone;
                //reponse.Question1 = Q1SelectedYesNo.Text;
                //reponse.Question2 = Q2SelectedYesNo.Text;

                //var _interviewArrondissement = reponse.InterviewArrondissement;
                //var _interviewStationMetro = reponse.InterviewStationMetro;

                //// -- On ajoute la réponse
                //await QuestionnaireManager.Instance.AddReponse(reponse);

                //var dialog = DependencyService.Get<IDialogService>();
                //dialog.Show("Fin du questionnaire", "Souhaitez-vous refaire une interview au même endroit ?", "OUI", "NON", async (res) =>
                //{
                //if (res)
                //{
                //    //QuestionnaireManager.Instance.ReInitInterview();
                //    //for (int i = _nav.NavigationStack.Count - 2; i >= 0; i--)
                //    //{
                //    //    var p = _nav.NavigationStack[i];
                //    //    if (p is QuestionPage)
                //    //    {
                //    //        _nav.RemovePage(p);
                //    //    }
                //    //    //if (p is InterviewedStep2Page)
                //    //    //{
                //    //    //    // -- On reinitialise la réponse
                //    //    //    ((InterviewedStep2Page)p).ReInitInterview(new Reponse() { InterviewArrondissement = _interviewArrondissement, InterviewStationMetro = _interviewStationMetro });
                //    //    //}
                //    //}
                //    await _nav.PopToRootAsync();
                //}
                //else
                //{
                //    await _nav.PopToRootAsync();
                //}
                //});

            });

        }

        void Q1YesNoSelected(object obj)
        {
            var tag = (TagViewModel)obj;

            tag.TextColor = Color.White;
            tag.BackgroundColor = Color.FromHex("#174163");
            var list = Q1YesNoItems.Where(d => d.Text != tag.Text).ToList();
            foreach (var elt in list)
            {
                elt.TextColor = Color.FromHex("#174163");
                elt.BackgroundColor = Color.White;
            }

            Q1SelectedYesNo = tag;
        }

        void Q2YesNoSelected(object obj)
        {
            var tag = (TagViewModel)obj;

            tag.TextColor = Color.White;
            tag.BackgroundColor = Color.FromHex("#174163");
            var list = Q2YesNoItems.Where(d => d.Text != tag.Text).ToList();
            foreach (var elt in list)
            {
                elt.TextColor = Color.FromHex("#174163");
                elt.BackgroundColor = Color.White;
            }

            Q2SelectedYesNo = tag;
        }
    }
}
