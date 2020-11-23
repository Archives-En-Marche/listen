using System;
using System.Collections.Generic;
using Listen.Models.WebServices;
using Listen.Views.ViewCells;
using Newtonsoft.Json;
using PopolLib.Contracts.FastListView;
using Xamarin.Forms;
using Survey = Listen.Models.RealmObjects.Survey;

namespace Listen.ViewModels.ViewCell
{
    public class SurveyViewCellViewModel : IFastViewCell
    {
        public string Text { get; set; }

        public string Count { get; set; }

        public Survey Survey { get; set; }

        public bool IsNational { get; set; }

        public SurveyViewCellViewModel(Survey survey)
        {
            Text = survey.Name;

            var questions = JsonConvert.DeserializeObject<List<Question>>(survey.Questions);
            Count = questions?.Count.ToString() + " questions";

            Survey = survey;

            IsNational = survey.Type == "national" ? true : false;
        }

        public DataTemplate ItemTemplate { get => new DataTemplate(typeof(SurveyViewCell)); set => throw new NotImplementedException(); }
    }
}

