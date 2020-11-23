using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.Contracts;
using Listen.Helpers;
using Listen.Managers;
using Listen.Models.WebServices;
using Listen.ViewModels.Tags;
using Listen.Views;
using PopolLib.Services;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class QuestionPageViewModel : ViewModelBase, IResetQuestion
    {
        INavigation _nav;

        Color _nextButtonColor;
        public Color NextButtonColor
        {
            get
            {
                return _nextButtonColor;
            }
            set
            {
                Set(() => NextButtonColor, ref _nextButtonColor, value);
            }
        }

        string _reponseLibre;
        public string ReponseLibre
        {
            get
            {
                return _reponseLibre;
            }
            set
            {
                Set(() => ReponseLibre, ref _reponseLibre, value);
            }
        }

        bool _isReponseLibreVisible;
        public bool IsReponseLibreVisible
        {
            get
            {
                return _isReponseLibreVisible;
            }
            set
            {
                Set(() => IsReponseLibreVisible, ref _isReponseLibreVisible, value);
            }
        }

        string _questionLabel;
        public string QuestionLabel
        {
            get
            {
                return _questionLabel;
            }
            set
            {
                Set(() => QuestionLabel, ref _questionLabel, value);
            }
        }

        public ICommand NextQuestion { get; set; }

        IList<TagViewModel> _tagList;
        public IList<TagViewModel> TagList
        {
            get
            {
                return _tagList;
            }
            set
            {
                Set(() => TagList, ref _tagList, value);
            }
        }

        public ICommand BackHome { get; set; }

        public ICommand TagCommand { get; set; }

        public IList<TagViewModel> SelectedTags { get; set; }

        Question question;

        public QuestionPageViewModel(INavigation nav, bool isFirstQuestion = false)
        {
            _nav = nav;
            if (isFirstQuestion)
            {
                SurveyEngineManager.Instance.InitCurrentSurvey();
            }
            question = SurveyEngineManager.Instance.GetNextQuestion();

            var questions = SurveyEngineManager.Instance.Questions;
            var current = SurveyEngineManager.Instance.QuestionNumber;

            Debug.WriteLine("Question ({0})", current);

            NextButtonColor = Color.FromHex("#ffeae0");

            if (current < questions.Count)
            {
                QuestionLabel = question.Content;
                if (!string.IsNullOrEmpty(question.Type))
                {
                    if (question.Type.Trim().ToLower() == "simple_field")
                    {
                        // -- Question Libre
                        IsReponseLibreVisible = true;
                        NextButtonColor = Color.FromHex("#fcd2be");
                    }
                    else
                    {
                        IsReponseLibreVisible = false;
                        // -- Choix
                        TagList = new ObservableCollection<TagViewModel>();
                        var choix = question.Choices;
                        foreach (var c in choix)
                        {
                            var tag = new TagViewModel()
                            {
                                Id = c.Id,
                                Text = c.Content,
                                Parameters = question,
                                TextColor = Color.FromHex("#174163"),
                                BackgroundColor = Color.White,
                            };
                            TagList.Add(tag);
                        }

                        SelectedTags = new List<TagViewModel>();
                    }
                }
                else
                {
                    // -- Alert ! Probleme sur le questionnaire
                }
            }

            TagCommand = new Command(TagSelected);

            NextQuestion = new Command(async (obj) =>
            {
                bool validated = true;
                // -- On checke
                if (!string.IsNullOrEmpty(question.Type))
                {
                    if (question.Type.Trim().ToLower() == "simple_field")
                    {
                        // -- Question Libre
                        IsReponseLibreVisible = true;

                        SurveyEngineManager.Instance.CurrentReply.Answers.Add(new Answer()
                        {
                            SurveyQuestion = question.Id,
                            TextField = ReponseLibre,
                            SelectedChoices = null
                        });

                        if (string.IsNullOrEmpty(ReponseLibre))
                        {
                            ReponseLibre = "";
                        }
                    }
                    else
                    {
                        IsReponseLibreVisible = false;
                        // -- Choix
                        if (SelectedTags?.Count == 0)
                        {
                            validated = false;
                        }

                        var list = new List<string>();

                        // -- Evite doublons sur Android - Return Back Button
                        SurveyEngineManager.Instance.CurrentReply.Answers.Remove(
                            SurveyEngineManager.Instance.CurrentReply.Answers.FirstOrDefault(q => q.SurveyQuestion == question.Id)
                            );
                        SurveyEngineManager.Instance.CurrentReply.Answers.Add(new Answer()
                        {
                            SurveyQuestion = question.Id,
                            TextField = null,
                            SelectedChoices = SelectedTags.Select(_t => _t.Id).ToList()
                        });
                    }
                }

                if (validated)
                {
                    if (current == questions.Count - 1)
                    {
                        await _nav.PushAsync(new RgpdPage(new RgpdPageViewModel(_nav)));
                    }
                    else
                    {
                        await _nav.PushAsync(new QuestionPage(new QuestionPageViewModel(_nav)));
                    }
                }
            });

            BackHome = new Command(async (obj) =>
            {
                var dialog = DependencyService.Get<IDialogService>();
                dialog.Show("Retour accueil", "Souhaitez-vous effacer les questions déjà remplies et revenir au choix du questionnaire ?", "Oui", "Non", (res) =>
                {
                    if (res)
                    {
                        for (int i = _nav.NavigationStack.Count - 1; i >= 0; i--)
                        {
                            var p = _nav.NavigationStack[i];
                            if (!(p is HomePage) && !(p is SurveyPage) && !(p is IntroPage))
                            {
                                _nav.RemovePage(p);
                                SurveyEngineManager.Instance.Rewind();
                            }
                        }
                        _nav.PopAsync();
                    }
                });
            });
        }

        private void TagSelected(object obj)
        {
            var tag = (TagViewModel)obj;
            var _question = tag.Parameters as Question;
            // -- Question choix simple ou multiple
            if (_question.Type.Trim().ToLower() == "multiple_choice")
            {
                // -- Multiple
                if (tag.BackgroundColor == Color.FromHex("#174163"))
                {
                    // -- OFF
                    tag.TextColor = Color.FromHex("#174163");
                    tag.BackgroundColor = Color.White;
                    // -- On remove le tag
                    SelectedTags.Remove(tag);
                }
                else
                {
                    // -- ON
                    tag.TextColor = Color.White;
                    tag.BackgroundColor = Color.FromHex("#174163");
                    // -- On ajoute le tag
                    SelectedTags.Add(tag);
                }

                if (SelectedTags.Count > 0)
                {
                    NextButtonColor = Color.FromHex("#fcd2be");
                }
                else
                {
                    NextButtonColor = Color.FromHex("#ffeae0");
                }
            }
            else
            {
                // -- Simple
                tag.TextColor = Color.White;
                tag.BackgroundColor = Color.FromHex("#174163");
                var list = TagList.Where(d => d.Text != tag.Text).ToList();

                SelectedTags.Clear();
                SelectedTags.Add(tag);

                foreach (var elt in list)
                {
                    elt.TextColor = Color.FromHex("#174163");
                    elt.BackgroundColor = Color.White;
                }

                if (SelectedTags.Count > 0)
                {
                    NextButtonColor = Color.FromHex("#fcd2be");
                }
                else
                {
                    NextButtonColor = Color.FromHex("#ffeae0");
                }
            }
        }

        public void Reset()
        {
            NextButtonColor = Color.FromHex("#ffeae0");
            ReponseLibre = "";
            var questions = SurveyEngineManager.Instance.Questions;
            var current = SurveyEngineManager.Instance.QuestionNumber;

            NextButtonColor = Color.FromHex("#ffeae0");

            if (question != null && current <= questions.Count)
            {
                QuestionLabel = question.Content;
                // -- Pour tests Multilignes et AutoAdjustFontSize
                //QuestionLabel = "Normally, the label text is drawn with the font you specify in the font property. If this property is set to true, and the text in the text property exceeds the label’s bounding rectangle, the label reduces the font size until the string fits or the minimum font scale is reached.";

                if (!string.IsNullOrEmpty(question.Type))
                {
                    if (question.Type.Trim().ToLower() == "simple_field")
                    {
                        // -- Question Libre
                        IsReponseLibreVisible = true;
                        NextButtonColor = Color.FromHex("#fcd2be");
                    }
                    else
                    {
                        IsReponseLibreVisible = false;
                        // -- Choix
                        TagList = new ObservableCollection<TagViewModel>();
                        var choix = question.Choices;
                        foreach (var c in choix)
                        {
                            var tag = new TagViewModel()
                            {
                                Id = c.Id,
                                Text = c.Content,
                                Parameters = question,
                                TextColor = Color.FromHex("#174163"),
                                BackgroundColor = Color.White,
                            };
                            TagList.Add(tag);
                        }

                        SelectedTags = new List<TagViewModel>();
                    }
                }
                else
                {
                    // -- Alert ! Probleme sur le questionnaire
                }
            }
        }
    }
}
