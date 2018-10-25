using System;
using Listen.Views.ViewCells;
using PopolLib.Contracts.FastListView;
using Xamarin.Forms;

namespace Listen.ViewModels.ViewCell
{
    public class NoSurveyViewCellViewModel : IFastViewCell
    {
        public DataTemplate ItemTemplate { get => new DataTemplate(typeof(NoSurveyViewCell)); set => throw new NotImplementedException(); }
    }
}
