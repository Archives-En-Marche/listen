using GalaSoft.MvvmLight;
using Listen.VisualElements;

namespace Listen.Views
{
    public partial class MoreInfosPage : BaseContentPage
    {
        public MoreInfosPage(ViewModelBase vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
