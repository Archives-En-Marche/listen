using Android.App;
using Android.Content.PM;
using Android.OS;
using System.Globalization;
using System.Threading;
using Android.Graphics.Drawables;
using Android.Content;
using Xamarin.Forms;
using Listen.Models.Tasks;
using Listen.Droid.Tasks;

namespace Listen.Droid
{
    [Activity(Label = "J'écoute", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", message =>
            {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                var parcelable = new UploadLongRunningTaskParcelable();
                parcelable.Task = new UploadLongRunningTask();
                intent.PutExtra("task", parcelable);
                //StartService(intent);
                LongRunningTaskService.EnqueueWork(this, intent);
            });

            MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message =>
            {
                var intent = new Intent(this, typeof(LongRunningTaskService));
                StopService(intent);
            });

            var fr = new CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentCulture = fr;

            // --typeof(LetterSpacingLabelRenderer)
            var t = typeof(PopolLib.Droid.Renderers.LetterSpacingLabelRenderer);

            PopolLib.Droid.Renderers.FastListView.FastListViewRenderer.Init();
            PopolLib.Droid.Platforms.Platform.Init(this, savedInstanceState);

            this.Window.SetBackgroundDrawable(new ColorDrawable() { Color = Android.Graphics.Color.White });

            App.ScreenSize = new Xamarin.Forms.Size(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density,
                Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);

            LoadApplication(new App());
        }
    }
}