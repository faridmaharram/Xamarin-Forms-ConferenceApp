using ConferenceApp.Database;
using ConferenceApp.Design;
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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        private void OnCreateConference(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SignUpPage());
        }
        private async Task OnJoinConference(object sender, EventArgs e)
        {
            DBase db = new DBase();


            _loading.IsRunning = true;



            List<Conference> list = await db.getConferanceConfCode(_conferenceCode.Text);

           

            if (list.Count != 0 && list[0].Status==true)
            {
                User.Email = _memberEmail.Text;

                string gonderilecekKey = list[0].Key;

                Conference c = new Conference();
                c.Key = list[0].Key;
                c.ConferenceCode = list[0].ConferenceCode;
                c.ConferenceName = list[0].ConferenceName;
                c.CreaterName = list[0].CreaterName;
                c.CreaterSurname = list[0].CreaterSurname;
                c.CreatorEmail = list[0].CreatorEmail;


                db.JoinConference(new Members { MemberEmail = _memberEmail.Text, ConfCode = _conferenceCode.Text }, gonderilecekKey);




                _loading.IsRunning = false;

                await Navigation.PushModalAsync(new MainPageUser());
                MessagingCenter.Send<LoginPage, Conference>(this, "Conferenceprop", c);
            }
            else
            {
                _loading.IsRunning = false;

                await DisplayAlert(AppResources.Result, AppResources.ResultUnsaccess, AppResources.Ok);
                ;
            }









        }
    }
}