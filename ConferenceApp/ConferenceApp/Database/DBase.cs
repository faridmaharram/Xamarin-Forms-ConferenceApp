using ConferenceApp.Models;
using Firebase.Xamarin.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceApp.Database
{
    public class DBase

    {
        FirebaseClient fbClient;


        public DBase()
        {

            fbClient = new FirebaseClient("https://conference-686e4.firebaseio.com/");
        }



        public async Task<List<Conference>> getConferanceConfCode(string confcode)
        {
            return (await fbClient.Child("Conference").OnceAsync<Conference>()).Where(m => m.Object.ConferenceCode == confcode)
               .Select((item) =>

               new Conference
               {
                   Key = item.Key,
                   CreaterName = item.Object.CreaterName,
                   CreaterSurname = item.Object.CreaterSurname,
                   CreatorEmail = item.Object.CreatorEmail,
                   ConferenceName = item.Object.ConferenceName,
                   ConferenceCode = item.Object.ConferenceCode,
                   Status=item.Object.Status  



               }

                    ).ToList();



        }

        public async Task<List<Questions>> getQuestion(string confcode)
        {
            return (await fbClient.Child("Conference/"+confcode+"Questions").OnceAsync<Questions>()).OrderByDescending(m=>m.Object.Point).Take(5).
                Select((item) =>

               new Questions
               {
                   Key = item.Key,
                    UserEmail= item.Object.UserEmail,
                     Point= item.Object.Point,
                      Question= item.Object.Question
            }

                    ).ToList();



        }


        public async Task AddConference(Conference cf)
        {

            await fbClient.Child("Conference").PostAsync(cf);
        }

        public void JoinConference(Members mb, string ConfKey)
        {



            fbClient.Child("Conference/" + ConfKey + "/Members").PostAsync(mb);
        }

        public async Task AddQuestions(Questions question, string ConfKey)
        {
            await fbClient.Child("Conference/" + ConfKey + "/Questions").PostAsync(question);
        }

        public ObservableCollection<Questions> ListQuestions(string ConfKey)
        {
            return fbClient.Child("Conference/" + ConfKey + "/Questions").AsObservable<Questions>().AsObservableCollection<Questions>();
        }
    }
}
