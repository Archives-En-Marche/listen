using System;
using Listen.Views;
using Xamarin.Forms;

namespace Listen.VisualElements
{
    public class InternalNavigationPage : NavigationPage
    {
        public InternalNavigationPage()
        {
            BarBackgroundColor = Color.White;
            BarTextColor = Color.White;
        }

        public InternalNavigationPage(Page root) : base(root)
        {
            BarBackgroundColor = Color.White;
            BarTextColor = Color.White;
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (!(child is HomePage))
            {
                this.BarBackgroundColor = Color.FromHex("#243366");
                this.BarTextColor = Color.White;
            }
            else
            {

            }
        }

    }
}
