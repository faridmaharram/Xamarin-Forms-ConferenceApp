using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ConferenceApp.Localization;
using System.Threading;
using Xamarin.Forms;
using ConferenceApp.Droid.LocalizeDependency;

[assembly: Dependency(typeof(Localize))]
namespace ConferenceApp.Droid.LocalizeDependency
{
    public class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLocale = androidLocale.ToString().Replace("_", "-");
            return new CultureInfo(netLocale);
        }

        public void SetLocale()
        {
            var androidLocale = Java.Util.Locale.Default;
            var netLocale = androidLocale.ToString().Replace("_", "-");
            var ci = new CultureInfo(netLocale);
            Thread.CurrentThread.CurrentCulture=ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}