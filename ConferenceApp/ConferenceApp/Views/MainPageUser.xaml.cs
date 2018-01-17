using ConferenceApp.Database;
using ConferenceApp.Design;
using ConferenceApp.Models;
using ConferenceApp.Resx;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Plugin.Share;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConferenceApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageUser : ContentPage
    {
        Conference cf;
        DBase db;
       
        bool durum = true;

        ObservableCollection<Questions> question = new ObservableCollection<Questions>();
        ObservableCollection<Questions> question2 = new ObservableCollection<Questions>();

        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    var fbClient = new FirebaseClient("https://conference-686e4.firebaseio.com/");

        //    var items = fbClient.Child("Conference/" + ConferenceKey.ConKey + "/Questions").OnceAsync<Questions>();

        //    foreach (var item in await items)
        //    {
        //        Questions q = new Questions();

        //        q.Key = item.Key;
        //        q.Point = item.Object.Point;
        //        q.Question = item.Object.Question;
        //        q.UserEmail = item.Object.UserEmail;

        //      await  fbClient.Child("Conference/" + ConferenceKey.ConKey + "/Questions/" + q.Key).Child("Key").PutAsync(q.Key);
        //    }



        //}


        public MainPageUser()
        {

            InitializeComponent();
            _loading.IsRunning = true;
            MessagingCenter.Subscribe<LoginPage, Conference>(this, "Conferenceprop", (page, data) => {
              
                cf = data;
                ConferenceKey.ConKey = cf.Key;
                _ConfName.Text = cf.ConferenceName;
                var fbClient = new FirebaseClient("https://conference-686e4.firebaseio.com/");
                question = fbClient.Child("Conference/" + cf.Key + "/Questions").AsObservable<Questions>()
                .AsObservableCollection<Questions>();
               
               

                _lstQuestions.ItemsSource = question;
                //_UserCount.Text =question.Count().ToString();

                MessagingCenter.Unsubscribe<LoginPage, Conference>(this, "Conferenceprop");
                
            });
            _loading.IsRunning = false;

        }


        private void onShare(object sender, EventArgs f)
        {




            CrossShare.Current.Share("Share Conference", "Conference");


        }

        private async Task onLogout(object sender, EventArgs f)
        {
            var fbClient = new FirebaseClient("https://conference-686e4.firebaseio.com/");


            if (User.Email == cf.CreatorEmail)
            {

                fbClient.Child("Conference/" + ConferenceKey.ConKey).Child("Status").PutAsync(false);

                var userList = (await fbClient.Child("Conference/" + ConferenceKey.ConKey + "/Members").OnceAsync<Members>()).
          Select((item) =>

         new Members
         {

             MemberEmail = item.Object.MemberEmail

         }

              ).ToList();




                var questionList = (await fbClient.Child("Conference/" + ConferenceKey.ConKey + "/Questions").OnceAsync<Questions>()).OrderByDescending(m => m.Object.Point).Take(5).
                    Select((item) =>

                   new Questions
                   {
                       Key = item.Key,
                       UserEmail = item.Object.UserEmail,
                       Point = item.Object.Point,
                       Question = item.Object.Question
                   }

                        ).ToList();




                string text = "Konferans Ismi: " + cf.ConferenceName + "  " + "Konferans Yöneticisi: " + cf.CreaterName + " " + cf.CreaterSurname + " Toplam kullanici sayi: " + userList.Count();

                //Creates a new PDF document.
                PdfDocument doc = new PdfDocument();

                //Adds a page.
                PdfPage page = doc.Pages.Add();

                //create a new PDF string format
                PdfStringFormat drawFormat = new PdfStringFormat();
                drawFormat.WordWrap = PdfWordWrapType.Word;
                drawFormat.Alignment = PdfTextAlignment.Justify;
                drawFormat.LineAlignment = PdfVerticalAlignment.Top;

                //Set the font.
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 10f);

                //Create a brush.
                PdfBrush brush = PdfBrushes.Red;

                //bounds
                RectangleF bounds = new RectangleF(new PointF(10, 10), new SizeF(page.Graphics.ClientSize.Width - 30, page.Graphics.ClientSize.Height - 20));

                //Create a new text elememt
                PdfTextElement element = new PdfTextElement(text, font, brush);

                //Set the string format
                element.StringFormat = drawFormat;

                //Draw the text element
                PdfLayoutResult result = element.Draw(page, bounds);

                // Draw the string one after another.

                // Creates a PdfLightTable.
                PdfLightTable QuestionTable = new PdfLightTable();

                //Add colums to light table
                QuestionTable.Columns.Add(new PdfColumn("Gönderen Kullanıcı"));
                QuestionTable.Columns.Add(new PdfColumn("Soru"));
                QuestionTable.Columns.Add(new PdfColumn("Puan"));


                for (int i = 0; i < questionList.Count(); i++)
                {
                    QuestionTable.Rows.Add(new string[] { questionList[i].UserEmail, questionList[i].Question, questionList[i].Point.ToString() });
                }


                QuestionTable.Style.ShowHeader = true;


                result = QuestionTable.Draw(page, new PointF(result.Bounds.Left, result.Bounds.Bottom + 20));


                PdfLightTable UserTable = new PdfLightTable();

                //Add colums to light table
                UserTable.Columns.Add(new PdfColumn("Kullanıcı Epostas"));



                for (int i = 0; i < userList.Count(); i++)
                {
                    UserTable.Rows.Add(new string[] { userList[i].MemberEmail });
                }


                UserTable.Style.ShowHeader = true;


                result = UserTable.Draw(page, new PointF(result.Bounds.Left, result.Bounds.Bottom + 20));



                MemoryStream stream = new MemoryStream();


                doc.Save(stream);

                doc.Close(true);

                await DependencyService.Get<ISave>().SaveTextAsync("Conference.pdf", "application/pdf", stream);


            }
            else
            {
             await   Navigation.PushModalAsync(new LoginPage());
            }




        }

        public async Task onQuestionAdd(object sender, EventArgs f)
        {
            _loading.IsRunning = true;
            var fbClient = new FirebaseClient("https://conference-686e4.firebaseio.com/");
            string cKey = ConferenceKey.ConKey;


         

             fbClient.Child("Conference/" + cKey + "/Questions").PostAsync(new Questions { Question = _EntQuestion.Text, UserEmail = User.Email });



            var items = fbClient.Child("Conference/" + ConferenceKey.ConKey + "/Questions").OnceAsync<Questions>();

            foreach (var item in await items)
            {
                Questions q = new Questions();
                QuestionKey.QueKey = item.Key;
                q.Key = item.Key;
             
                await fbClient.Child("Conference/" + ConferenceKey.ConKey + "/Questions/" + q.Key).Child("Key").PutAsync(q.Key);
            }


            _loading.IsRunning = false;
            DisplayAlert(AppResources.Result, AppResources.ResultSuccess, AppResources.Ok);
            await Navigation.PushAsync(new MainPageUser());
            _EntQuestion.Text = null;

        }

        private async void onItemSelected(object sender, SelectedItemChangedEventArgs f)
        {
           
            if (f.SelectedItem == null)
                return;

            ListView lv = (ListView)sender;

            var selectedQuestion = (Questions)f.SelectedItem;



            Navigation.PushModalAsync(new QuestionDetail(selectedQuestion));


            lv.SelectedItem = null;
        }

        private void OnScoreQuestion(object sender, SelectedItemChangedEventArgs f)
        {
            var fbClient = new FirebaseClient("https://conference-686e4.firebaseio.com/");


            var item = (Button)sender;
           // var den= item.BindingContext ;
            Questions deniz = (Questions)item.BindingContext;
           
            var yeni = question.Where(m => m.Key == deniz.Key).FirstOrDefault();

            
            var key = yeni.Key;
            //if (item.Text == "+")
            //{               
                yeni.Point = 1 + yeni.Point;
                deniz.Point = yeni.Point;

              fbClient.Child("Conference/" + ConferenceKey.ConKey + "/Questions/" + key+"/").Child("Point").PutAsync(deniz.Point);


                //item.Text = "-";
                //durum = false;
               

            //}
            //else
            //{              
            //    yeni.Point = yeni.Point-1;
            //    deniz.Point = yeni.Point;
            //    fbClient.Child("Conference/" + ConferenceKey.ConKey + "/Questions/" + key + "/").Child("Point").PutAsync(deniz.Point);

            //    item.Text = "+";
            //    durum = true;
             
            //}

           
       
        }
    }
}