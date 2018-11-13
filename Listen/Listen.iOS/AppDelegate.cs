using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Foundation;
using Listen.iOS.Tasks;
using Listen.Models.Tasks;
using PopolLib.iOS.Services;
using PopolLib.Services;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Listen.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        LongRunningTask upload_task;

        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            //Xamarin.Calabash.Start();
#if ENABLE_TEST_CLOUD
            // requires Xamarin Test Cloud Agent
            Xamarin.Calabash.Start();
#endif

            var normalTextAttributes = new UITextAttributes();
       
            normalTextAttributes.Font = UIFont.FromName("Roboto-Bold", 16.0f); // -- Unselected
            normalTextAttributes.TextColor = Color.FromHex("#174163").ToUIColor();
            UINavigationBar.Appearance.SetTitleTextAttributes(normalTextAttributes);

            UINavigationBar.Appearance.BarTintColor = Color.FromHex("#eff9ff").ToUIColor();
            UINavigationBar.Appearance.TintColor = Color.FromHex("#174163").ToUIColor();

            UINavigationBar.Appearance.ShadowImage = new UIImage();

            global::Xamarin.Forms.Forms.Init();

            MessagingCenter.Subscribe<StartLongRunningTaskMessage>(this, "StartLongRunningTaskMessage", async message => {
                upload_task = new LongRunningTask(new UploadLongRunningTask());
                await upload_task.Start();
            });

            MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, "StopLongRunningTaskMessage", message => {
                upload_task.Stop();
            });

            //DependencyService.Register<IProgressHUD, IOSProgressHUD>();

            App.ScreenSize = new Size(UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

            var fr = new CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentCulture = fr;
            PopolLib.iOS.Renderers.FastListView.FastListViewRenderer.Init();

            LoadApplication(new App());

            // -- Liste des Fonts dans l'application
            //var fontList = new StringBuilder();
            //var familyNames = UIFont.FamilyNames;
            //foreach (var familyName in familyNames)
            //{
            //    fontList.Append(String.Format("Family: {0}\n", familyName));
            //    Console.WriteLine("Family: {0}\n", familyName);
            //    var fontNames = UIFont.FontNamesForFamilyName(familyName);
            //    foreach (var fontName in fontNames)
            //    {
            //        Console.WriteLine("\tFont: {0}\n", fontName);
            //        fontList.Append(String.Format("\tFont: {0}\n", fontName));
            //    }
            //};

            //Console.WriteLine(fontList.ToString());
            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
