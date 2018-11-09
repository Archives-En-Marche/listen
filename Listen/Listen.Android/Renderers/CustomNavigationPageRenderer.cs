using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Listen.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Support = Android.Support.V7.Widget;
using AToolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Listen.Views;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationPageRenderer))]
namespace Listen.Droid.Renderers
{
    public class CustomNavigationPageRenderer : NavigationPageRenderer
    {
        public AToolbar _toolbar;
        public Context _context;

        public CustomNavigationPageRenderer(Context context) : base(context)
        {
            _context = context;
        }


        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);

            //if (child.GetType() == typeof(Support.Toolbar))
            //_toolbar = (Support.Toolbar)child;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var page = this.Element as NavigationPage;

            //if (page != null && _toolbar != null)
            //{
            //Typeface tf = Typeface.CreateFromAsset(Android.App.Application.Context.Assets, "gillsans-regular.ttf");
            //TextView title = (TextView)_toolbar.FindViewById(Resource.Id.toolbar);
            //title.SetText(page.CurrentPage.Title, TextView.BufferType.Normal);
            //title.SetTypeface(tf, TypefaceStyle.Normal);
            //}
        }

        protected override Task<bool> OnPushAsync(Page view, bool animated)
        {
            var retVal = base.OnPushAsync(view, animated);

            var activity = (Activity)_context;
            _toolbar = activity.FindViewById<AToolbar>(Resource.Id.toolbar);

            if (_toolbar != null && view is IntroPage)
            {
                if (_toolbar.NavigationIcon != null)
                {
                    _toolbar.NavigationIcon = Android.Support.V4.Content.ContextCompat.GetDrawable(_context, Resource.Drawable.home);
                    //toolbar.SetNavigationIcon(Resource.Drawable.Back);
                }
            }
            return retVal;
            //return base.OnPushAsync(view, animated);
        }

    }
}