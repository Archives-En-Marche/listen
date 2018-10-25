using System;
using Listen.Views.ViewCells;
using PopolLib.Contracts.FastListView;
using Xamarin.Forms;

namespace Listen.ViewModels.ViewCell
{
    public class UserWelcomeViewCellViewModel : IFastViewCell
    {
        public string Fullname { get; set; }

        public string Departement { get; set; }

        public UserWelcomeViewCellViewModel(string fullname, string dept)
        {
            Fullname = "Bonjour " + fullname + ",";
            Departement = "Voici les questionnaires disponibles dans votre département (" + dept + "). ";
        }

        public DataTemplate ItemTemplate { get => new DataTemplate(typeof(UserWelcomeViewCell)); set => throw new NotImplementedException(); }
    }
}
