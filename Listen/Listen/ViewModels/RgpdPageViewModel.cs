using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.Helpers;
using Listen.Managers;
using Listen.ViewModels.Tags;
using Listen.Views;
using PopolLib.Services;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class RgpdPageViewModel : ViewModelBase
    {
        INavigation _nav;

        TagViewModel _selectedQ1;
        TagViewModel _selectedQ2;
        TagViewModel _selectedQ3;

        IList<TagViewModel> _q1Items;
        public IList<TagViewModel> Q1Items { get { return _q1Items; } set { Set(() => Q1Items, ref _q1Items, value); } }
        IList<TagViewModel> _q2Items;
        public IList<TagViewModel> Q2Items { get { return _q2Items; } set { Set(() => Q2Items, ref _q2Items, value); } }
        IList<TagViewModel> _q3Items;
        public IList<TagViewModel> Q3Items { get { return _q3Items; } set { Set(() => Q3Items, ref _q3Items, value); } }

        public ICommand YesNoCommand { get; set; }

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

            Q1Items = new ObservableCollection<TagViewModel>();
            {
                var f = new TagViewModel
                {
                    Text = "Oui",
                    Parameters = 1,
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                Q1Items.Add(f);
            }
            {
                var f = new TagViewModel
                {
                    Text = "Non",
                    Parameters = 1,
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                Q1Items.Add(f);
            }

            Q2Items = new ObservableCollection<TagViewModel>();
            {
                var f = new TagViewModel
                {
                    Text = "Oui",
                    Parameters = 2,
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                Q2Items.Add(f);
            }
            {
                var f = new TagViewModel
                {
                    Text = "Non",
                    Parameters = 2,
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                Q2Items.Add(f);
            }

            Q3Items = new ObservableCollection<TagViewModel>();
            {
                var f = new TagViewModel
                {
                    Text = "Oui",
                    Parameters = 3,
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                Q3Items.Add(f);
            }
            {
                var f = new TagViewModel
                {
                    Text = "Non",
                    Parameters = 3,
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                Q3Items.Add(f);
            }

            YesNoCommand = new Command(YesNoSelected);

            ValiderCommand = new Command(async (obj) =>
            {
                var frame = (Frame)obj;
                ButtonAnimationHelper.Animate(frame);

                var emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

                if (string.IsNullOrEmpty(Email))
                {
                    Email = null;
                }



                bool ok = true;

                if (!string.IsNullOrEmpty(Email))
                {
                    ok  = (Regex.IsMatch(Email.Trim(), emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
                }

                if (_selectedQ2?.Text == "Oui" && string.IsNullOrEmpty(Email))
                {
                    ok = false;
                }

                if (_selectedQ3?.Text == "Oui" && string.IsNullOrEmpty(Email))
                {
                    ok = false;
                }

                if (_selectedQ1 != null && _selectedQ2 != null && _selectedQ3 != null && ok)
                {
                    SurveyEngineManager.Instance.CurrentReply.Firstname = Prenom;
                    SurveyEngineManager.Instance.CurrentReply.Lastmame = Nom;
                    SurveyEngineManager.Instance.CurrentReply.Email = Email;
                    SurveyEngineManager.Instance.CurrentReply.AgreedToStayInContact = (_selectedQ2?.Text == "Oui" ? true : false);
                    SurveyEngineManager.Instance.CurrentReply.AgreedToContactForJoin = (_selectedQ3?.Text == "Oui" ? true : false);
                    SurveyEngineManager.Instance.CurrentReply.AgreedToTreatPersonalData = (_selectedQ1?.Text == "Oui" ? true : false);

                    await _nav.PushAsync(new MoreInfosPage(new MoreInfosPageViewModel(_nav)));
                    //await _nav.PushAsync(new EndPage(new EndPageViewModel(_nav)));
                }
                else
                {
                    var dialog = DependencyService.Get<IDialogService>();
                    dialog.Show("Oups !", "Merci de renseigner toutes les informations sur cet écran.", "OK", null);
                }

            });

        }

        void YesNoSelected(object obj)
        {
            var tag = (TagViewModel)obj;

            tag.TextColor = Color.White;
            tag.BackgroundColor = Color.FromHex("#174163");
            IList<TagViewModel> list = null;

            switch ((int)tag.Parameters)
            {
                case 1:
                    list = Q1Items.Where(d => d.Text != tag.Text).ToList();
                    _selectedQ1 = tag;
                    break;
                case 2:
                    list = Q2Items.Where(d => d.Text != tag.Text).ToList();
                    _selectedQ2 = tag;
                    break;
                case 3:
                    list = Q3Items.Where(d => d.Text != tag.Text).ToList();
                    _selectedQ3 = tag;
                    break;
            }

            if (list?.Count > 0)
            {
                foreach (var elt in list)
                {
                    elt.TextColor = Color.FromHex("#174163");
                    elt.BackgroundColor = Color.White;
                }
            }

        }
    }
}
