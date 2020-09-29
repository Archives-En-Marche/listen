using Listen.Views;
using Xamarin.Forms;

namespace Listen.VisualElements
{
    public class InternalNavigationPage : NavigationPage
    {
        public InternalNavigationPage()
        {
            this.BarBackgroundColor = (Color)Application.Current.Resources["paleGrey"];
            this.BarTextColor = (Color)Application.Current.Resources["darkSlateBlue"];
        }

        public InternalNavigationPage(Page root) : base(root)
        {
            this.BarBackgroundColor = (Color)Application.Current.Resources["paleGrey"];
            this.BarTextColor = (Color)Application.Current.Resources["darkSlateBlue"];
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (!(child is HomePage))
            {
                this.BarBackgroundColor = (Color) Application.Current.Resources["paleGrey"];
                //this.BarBackgroundColor = Color.FromHex("#f6fbff");
                this.BarTextColor = (Color)Application.Current.Resources["darkSlateBlue"];
            }
        }
    }
}
