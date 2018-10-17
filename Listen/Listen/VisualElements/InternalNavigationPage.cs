using System;
using Listen.Views;
using Xamarin.Forms;

namespace Listen.VisualElements
{
    public class InternalNavigationPage : NavigationPage
    {
        public InternalNavigationPage()
        {
        }

        public InternalNavigationPage(Page root) : base(root)
        {

        }
        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (!(child is HomePage))
            {
                this.BarBackgroundColor = Color.FromHex("#243366");
            }
            else
            {

            }
        }

    }
}
