using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Foundation;
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
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Xamarin.Calabash.Start();

            var normalTextAttributes = new UITextAttributes();
            normalTextAttributes.Font = UIFont.FromName("Gill Sans", 20.0f); // -- Unselected
            normalTextAttributes.TextColor = UIColor.White;
            UINavigationBar.Appearance.SetTitleTextAttributes(normalTextAttributes);

            UINavigationBar.Appearance.BarTintColor = Color.FromHex("#243366").ToUIColor();
            UINavigationBar.Appearance.TintColor = UIColor.White;
           
            global::Xamarin.Forms.Forms.Init();

            DependencyService.Register<IProgressHUD, IOSProgressHUD>();

            App.ScreenSize = new Size(UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

            var fr = new CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentCulture = fr;

            LoadApplication(new App());

            // -- Liste des Fonts dans l'application
            var fontList = new StringBuilder();
            var familyNames = UIFont.FamilyNames;
            foreach (var familyName in familyNames)
            {
                fontList.Append(String.Format("Family: {0}\n", familyName));
                Console.WriteLine("Family: {0}\n", familyName);
                var fontNames = UIFont.FontNamesForFamilyName(familyName);
                foreach (var fontName in fontNames)
                {
                    Console.WriteLine("\tFont: {0}\n", fontName);
                    fontList.Append(String.Format("\tFont: {0}\n", fontName));
                }
            };

            Console.WriteLine(fontList.ToString());
            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}
