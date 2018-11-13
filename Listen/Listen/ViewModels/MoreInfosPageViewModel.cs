using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.Managers;
using Listen.ViewModels.Tags;
using Listen.Views;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class MoreInfosPageViewModel : ViewModelBase
    {
        INavigation _nav;

        TagViewModel _selectedJob;
        TagViewModel _selectedAge;
        TagViewModel _selectedGender;

        IList<TagViewModel> _jobItems;
        public IList<TagViewModel> JobItems { get { return _jobItems; } set { Set(() => JobItems, ref _jobItems, value); } }
        IList<TagViewModel> _ageItems;
        public IList<TagViewModel> AgeItems { get { return _ageItems; } set { Set(() => AgeItems, ref _ageItems, value); } }
        IList<TagViewModel> _genderItems;
        public IList<TagViewModel> GenderItems { get { return _genderItems; } set { Set(() => GenderItems, ref _genderItems, value); } }

        public ICommand SelectCommand { get; set; }
        public ICommand SoumettreCommand { get; set; }

        string[] ages = new string[] { "-20 ans", "20-24 ans", "25-39 ans", "40-54 ans", "55-64 ans", "65-80 ans", "80+ ans" };
        string[] jobs = new string[] { "Agriculteurs exploitants", "Artisans, commerçants et chefs d’entreprise",
            "Cadres et professions intellectuelles", "Professions Intermédiaires", "Employés", "Ouvriers", "Retraités",
        "Auto-Entrepreneurs", "Étudiants", "Parents au foyer", "Demandeurs d’emploi"};
        string[] genders = new string[] { "Féminin", "Masculin", "Autre" };

        Dictionary<string, string> _resultAges = new Dictionary<string, string>() { { "-20 ans", "less_than_20" }, { "20-24 ans", "between_20_24" },
            { "25-39 ans", "between_25_39" }, { "40-54 ans", "between_40_54" }, { "55-64 ans", "between_55_64" },
            { "65-80 ans", "between_65_80" }, { "80+ ans", "greater_than_80" } };

        Dictionary<string, string> _resultJobs = new Dictionary<string, string>() { { "Agriculteurs exploitants", "farmers" },
            { "Artisans, commerçants et chefs d’entreprise", "craftsmen" }, { "Cadres et professions intellectuelles", "managerial staff" },
            { "Professions Intermédiaires", "intermediate_professions" }, { "Employés", "employees" }, { "Ouvriers", "workers" },
            { "Retraités", "retirees" }, { "Auto-Entrepreneurs", "self_contractor" }, { "Étudiants", "student" },
            { "Parents au foyer", "home_parent" },  { "Demandeurs d’emploi", "jobseeker" } };

        string _postalCode;
        public string PostalCode
        {
            get
            {
                return _postalCode;
            }
            set
            {
                Set(() => PostalCode, ref _postalCode, value);
            }
        }

        string _gender;
        public string Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                Set(() => Gender, ref _gender, value);
            }
        }

        bool _genderIsVisible;
        public bool GenderIsVisible
        {
            get
            {
                return _genderIsVisible;
            }
            set
            {
                Set(() => GenderIsVisible, ref _genderIsVisible, value);
            }
        }

        public MoreInfosPageViewModel(INavigation navigation)
        {
            _nav = navigation;

            GenderIsVisible = false;

            JobItems = new ObservableCollection<TagViewModel>();
            foreach (var s in jobs)
            {
                var f = new TagViewModel
                {
                    Text = s,
                    Parameters = "jobs",
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                JobItems.Add(f);
            }

            AgeItems = new ObservableCollection<TagViewModel>();
            foreach (var s in ages)
            {
                var f = new TagViewModel
                {
                    Text = s,
                    Parameters = "ages",
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                AgeItems.Add(f);
            }

            GenderItems = new ObservableCollection<TagViewModel>();
            foreach (var s in genders)
            {
                var f = new TagViewModel
                {
                    Text = s,
                    Parameters = "genders",
                    TextColor = Color.FromHex("#174163"),
                    BackgroundColor = Color.White,
                };
                GenderItems.Add(f);
            }

            SelectCommand = new Command(SelectAction);
            SoumettreCommand = new Command(async (obj) =>
            {
                // -- Check
                //if (_selectedJob != null && _selectedAge != null && _selectedGender != null)
                //{
                    SurveyEngineManager.Instance.CurrentReply.PostalCode = PostalCode;

                    var labelAge = _resultAges.FirstOrDefault(a => a.Key == _selectedAge?.Text);
                    SurveyEngineManager.Instance.CurrentReply.AgeRange = labelAge.Value;

                    var labelJob = _resultJobs.FirstOrDefault(a => a.Key == _selectedJob?.Text);
                    SurveyEngineManager.Instance.CurrentReply.Profession = labelJob.Value;

                    // -- gender
                    var gender = _selectedGender?.Text;
                    if (gender == "Féminin")
                    {
                        SurveyEngineManager.Instance.CurrentReply.Gender = "female";
                        SurveyEngineManager.Instance.CurrentReply.GenderOther = null;
                    }
                    if (gender == "Masculin")
                    {
                        SurveyEngineManager.Instance.CurrentReply.Gender = "male";
                        SurveyEngineManager.Instance.CurrentReply.GenderOther = null;
                    }
                    if (gender == "Autre")
                    {
                        SurveyEngineManager.Instance.CurrentReply.Gender = "other";
                        SurveyEngineManager.Instance.CurrentReply.GenderOther = Gender;
                    }

                    await SurveyManager.Instance.AddReplyAsync(SurveyEngineManager.Instance.CurrentReply);
                    // -- Navigate
                    await _nav.PushAsync(new EndPage(new EndPageViewModel(_nav)));
                //}
            });

        }

        void SelectAction(object obj)
        {
            var tag = (TagViewModel)obj;

            tag.TextColor = Color.White;
            tag.BackgroundColor = Color.FromHex("#174163");
            IList<TagViewModel> list = null;

            switch ((string)tag.Parameters)
            {
                case "jobs":
                    list = JobItems.Where(d => d.Text != tag.Text).ToList();
                    _selectedJob = tag;
                    break;
                case "ages":
                    list = AgeItems.Where(d => d.Text != tag.Text).ToList();
                    _selectedAge = tag;
                    break;
                case "genders":
                    list = GenderItems.Where(d => d.Text != tag.Text).ToList();
                    _selectedGender = tag;
                    if (_selectedGender.Text == "AUTRE")
                    {
                        GenderIsVisible = true;
                    }
                    else
                    {
                        GenderIsVisible = false;
                    }
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
