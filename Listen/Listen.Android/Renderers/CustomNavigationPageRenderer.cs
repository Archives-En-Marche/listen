using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Listen.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using Support = Android.Support.V7.Widget;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationPageRenderer))]
namespace Listen.Droid.Renderers
{
    public class CustomNavigationPageRenderer : NavigationPageRenderer
    {
        private Support.Toolbar _toolbar;

        public CustomNavigationPageRenderer(Context context) : base(context)
        {
        }

        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);

            if (child.GetType() == typeof(Support.Toolbar))
                _toolbar = (Support.Toolbar)child;
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

    }
}