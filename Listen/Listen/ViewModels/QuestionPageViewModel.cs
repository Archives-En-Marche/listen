using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Listen.Managers;
using Listen.Models.WebServices;
using Listen.ViewModels.Tags;
using Listen.Views;
using PopolLib.Services;
using Xamarin.Forms;

namespace Listen.ViewModels
{
    public class QuestionPageViewModel : ViewModelBase
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

        public ICommand TagCommand { get; set; }

        public IList<string> SelectedTags { get; set; }



        public QuestionPageViewModel(INavigation nav)
        {
            _nav = nav;

            var question = SurveyEngineManager.Instance.GetNextQuestion();

            var questions = SurveyEngineManager.Instance.Questions;
            var current = SurveyEngineManager.Instance.QuestionNumber;

            NextButtonColor = Color.FromHex("#ffeae0");

            if (current <= questions.Count)
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

                        SelectedTags = new List<string>();
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

                        if (string.IsNullOrEmpty(ReponseLibre))
                        {
                            ReponseLibre = "";
                        }


                        //if (string.IsNullOrEmpty(reponse.Values))
                        //{
                        //    reponse.Values = "\"" + ReponseLibre + "\"";
                        //}
                        //else
                        //{
                        //    reponse.Values += ";\"" + ReponseLibre + "\"";
                        //}
                    }
                    else
                    {
                        IsReponseLibreVisible = false;
                        // -- Choix
                        if (SelectedTags?.Count == 0)
                        {
                            validated = false;
                            //if (question.Type.Trim().ToLower() == "multiple_choice")
                            //{
                            //    var dialog = DependencyService.Get<IDialogService>();
                            //    dialog.Show("Erreur", "Veuillez valider au moins un choix.", "Oui", (_obj) => { });

                            //}
                            //else 
                            //{
                            //    var dialog = DependencyService.Get<IDialogService>();
                            //    dialog.Show("Erreur", "Veuillez valider un choix.", "Oui", (_obj) => { });
                            //}
                        }

                        //if (string.IsNullOrEmpty(reponse.Values))
                        //{
                        //    reponse.Values = "\"" + string.Join(",", SelectedTags) + "\"";
                        //}
                        //else
                        //{
                        //    reponse.Values += ";\"" + string.Join(",", SelectedTags) + "\"";
                        //}
                    }
                }

                //NotifyTask .Builder
                //var hud = DependencyService.Get<IProgressHUD>();
                //hud.Show("Sauvegarde ...");
                //var question = questions[current];

                //if (!string.IsNullOrEmpty(question.Type))
                //{
                //    if (question.Type.Trim().ToLower() == "simple_field")
                //    {
                //        // -- Question Libre
                //        IsReponseLibreVisible = true;
                //        if (string.IsNullOrEmpty(reponse.Values))
                //        {
                //            reponse.Values = "\"" + ReponseLibre + "\"";
                //        }
                //        else
                //        {
                //            reponse.Values += ";\"" + ReponseLibre + "\"";
                //        }
                //    }
                //    else
                //    {
                //        IsReponseLibreVisible = false;
                //        // -- Choix
                //        if (string.IsNullOrEmpty(reponse.Values))
                //        {
                //            reponse.Values = "\"" + string.Join(",", SelectedTags) + "\"";
                //        }
                //        else
                //        {
                //            reponse.Values += ";\"" + string.Join(",", SelectedTags) + "\"";
                //        }
                //    }
                //}

                //hud.Dismiss();
                if (validated)
                {
                    if (current == questions.Count)
                    {
                        //await _nav.PushAsync(new RgpdPage(new RgpdPageViewModel(_nav, reponse)));
                        await _nav.PushAsync(new RgpdPage(new RgpdPageViewModel(_nav)));
                    }
                    else
                    {
                        //QuestionnaireManager.Instance.CurrentQuestion++;
                        await _nav.PushAsync(new QuestionPage(new QuestionPageViewModel(_nav)));
                    }
                }
            });
        }

        private void TagSelected(object obj)
        {
            var tag = (TagViewModel)obj;
            var question = tag.Parameters as Question;
            // -- Question choix simple ou multiple
            if (question.Type.Trim().ToLower() == "multiple_choice")
            {
                // -- Multiple
                if (tag.BackgroundColor == Color.FromHex("#174163"))
                {
                    // -- OFF
                    tag.TextColor = Color.FromHex("#174163");
                    tag.BackgroundColor = Color.White;
                    // -- On remove le tag
                    SelectedTags.Remove(tag.Text);
                }
                else
                {
                    // -- ON
                    tag.TextColor = Color.White;
                    tag.BackgroundColor = Color.FromHex("#174163");
                    // -- On ajoute le tag
                    SelectedTags.Add(tag.Text);
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
                SelectedTags.Add(tag.Text);

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
    }
}
