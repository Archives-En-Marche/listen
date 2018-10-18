using System;
using Listen.Models.RealmObjects;
using Listen.Views.ViewCells;
using PopolLib.Contracts.FastListView;
using Xamarin.Forms;

namespace Listen.ViewModels.ViewCell
{
    public class SurveyViewCellViewModel : IFastViewCell
    {
        public string Text { get; set; }

        public Survey Survey { get; set; }

        public SurveyViewCellViewModel(Survey survey)
        {
            Text = survey.Name;
            Survey = survey;
        }

        public DataTemplate ItemTemplate { get => new DataTemplate(typeof(SurveyViewCell)); set => throw new NotImplementedException(); }
    }
}

