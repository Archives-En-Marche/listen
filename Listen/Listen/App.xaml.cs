using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Listen.Helpers;
using Listen.Managers;
using Listen.Models.RealmAccess;
using Listen.Models.WebServices;
using Listen.Services;
using Listen.ViewModels;
using Listen.Views;
using Listen.VisualElements;
using PopolLib.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Listen
{
    public partial class App : Application, INavigation
    {
        public static Size ScreenSize { get; set; }

        public IReadOnlyList<Page> ModalStack => MainPage.Navigation.ModalStack;

        public IReadOnlyList<Page> NavigationStack => MainPage.Navigation.NavigationStack;

        public App()
        {
            InitializeComponent();

#if DEBUG
            Settings.AppSettings.AddOrUpdateValue("WS_BASE_URL", "https://staging.en-marche.fr");
            Settings.AppSettings.AddOrUpdateValue("GS_STORAGE_URI", "paris-et-moi.appspot.com");
#else
            Settings.AppSettings.AddOrUpdateValue("WS_BASE_URL", "https://www.en-marche.fr");
            Settings.AppSettings.AddOrUpdateValue("GS_STORAGE_URI", "paris-et-moi.appspot.com");
#endif
            Settings.AppSettings.AddOrUpdateValue("WS_TIME_OUT", 30000);
            Settings.AppSettings.AddOrUpdateValue("DB_NAME", "listen.realm");
            Settings.AppSettings.AddOrUpdateValue("WS_BASE_URL", "https://staging.en-marche.fr");
            Settings.AppSettings.AddOrUpdateValue("GS_STORAGE_URI", "paris-et-moi.appspot.com");

            MainPage = new InternalNavigationPage(new HomePage(this, new HomePageViewModel(this)));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            //var hud = DependencyService.Get<IProgressHUD>();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public void InsertPageBefore(Page page, Page before)
        {
            throw new NotImplementedException();
        }

        public async Task<Page> PopAsync()
        {
            return await MainPage.Navigation.PopAsync();
        }

        public async Task<Page> PopAsync(bool animated)
        {
            return await MainPage.Navigation.PopAsync(animated);
        }

        public async Task<Page> PopModalAsync()
        {
            return await MainPage.Navigation.PopModalAsync();
        }

        public async Task<Page> PopModalAsync(bool animated)
        {
            return await MainPage.Navigation.PopModalAsync(animated);
        }

        public async Task PopToRootAsync()
        {
            await MainPage.Navigation.PopToRootAsync();
        }

        public async Task PopToRootAsync(bool animated)
        {
            await MainPage.Navigation.PopToRootAsync(animated);
        }

        public async Task PushAsync(Page page)
        {
            await MainPage.Navigation.PushAsync(page);
        }

        public async Task PushAsync(Page page, bool animated)
        {
            await MainPage.Navigation.PushAsync(page, animated);
        }

        public Task PushModalAsync(Page page)
        {
            throw new NotImplementedException();
        }

        public Task PushModalAsync(Page page, bool animated)
        {
            throw new NotImplementedException();
        }

        public void RemovePage(Page page)
        {
            MainPage.Navigation.RemovePage(page);
        }
    }
}
