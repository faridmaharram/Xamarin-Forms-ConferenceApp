using ConferenceApp.Localization;
using ConferenceApp.Resx;
using ConferenceApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ConferenceApp
{
    public partial class App : Application
    {
        public App()
        {
            if(Device.OS==TargetPlatform.Android || Device.OS==TargetPlatform.iOS)
            {
                DependencyService.Get<ILocalize>().SetLocale();
                AppResources.Culture = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
