using System;
using System.Windows.Input;
using Listen.Views.ViewCells;
using PopolLib.Contracts.FastListView;
using Xamarin.Forms;

namespace Listen.ViewModels.ViewCell
{
    public class UpdateSurveyViewCellViewModel : IFastViewCell
    {
        public ICommand ActualiserCommand { get; set; }

        public DataTemplate ItemTemplate { get => new DataTemplate(typeof(UpdateSurveyViewCell)); set => throw new NotImplementedException(); }
    }
}
