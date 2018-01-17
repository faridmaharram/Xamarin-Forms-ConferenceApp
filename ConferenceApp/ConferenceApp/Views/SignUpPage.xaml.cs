using ConferenceApp.Database;
using ConferenceApp.Models;
using ConferenceApp.Resx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConferenceApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {

        public SignUpPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }
        public async Task onConferenceAdd(object sender, EventArgs f)
        {
            DBase db = new DBase();
            string strPassword = Guid.NewGuid().ToString().Substring(0, 4);
            string conCode = (DateTime.Now.Day + "0" + DateTime.Now.Month + "1" + DateTime.Now.Day).ToString() + strPassword;

            if (_ConferenceName.Text == null || _crEmail.Text == null || _crName.Text == null || _crSurname.Text == null)
            {
                await DisplayAlert("Hata", "Lütfen bütün alanları giriniz!", "Tamam");
            }
            else
            {
                _loading.IsRunning = true;

                try
                {
                    
                  

                    await db.AddConference(new Conference
                    {
                        CreaterName = _crName.Text,
                        CreaterSurname = _crSurname.Text,
                        CreatorEmail = _crEmail.Text,
                        ConferenceCode = conCode,
                        Status=true,
                        ConferenceName = _ConferenceName.Text

                    });
                    _loading.IsRunning = false;
                    await DisplayAlert(AppResources.Result+" : "+AppResources.ResultSuccess, AppResources.ConferenceCode +": " + conCode, AppResources.Ok);

                    await Navigation.PushModalAsync(new LoginPage());
                }
                catch (Exception e)
                {
                    await DisplayAlert(AppResources.Result + " : " + AppResources.ResultUnsaccess,"+: "+e, AppResources.Ok);
                    throw;
                }

            }




        }
    }
}