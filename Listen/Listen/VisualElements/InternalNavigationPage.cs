using System;
using Listen.Views;
using Xamarin.Forms;

namespace Listen.VisualElements
{
    public class InternalNavigationPage : NavigationPage
    {
        public InternalNavigationPage()
        {
            //this.BarBackgroundColor = Color.FromHex("#f6fbff");
            //this.BarTextColor = Color.FromHex("#174163");
        }

        public InternalNavigationPage(Page root) : base(root)
        {
            //this.BarBackgroundColor = Color.FromHex("#f6fbff");
            //this.BarTextColor = Color.FromHex("#174163");
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (!(child is HomePage))
            {
                this.BarBackgroundColor = Color.FromHex("#f6fbff");
                this.BarTextColor = Color.FromHex("#174163");
            }
            else
            {

            }
        }

    }
}
