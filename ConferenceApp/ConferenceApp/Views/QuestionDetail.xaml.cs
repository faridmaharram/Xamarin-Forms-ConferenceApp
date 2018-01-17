using ConferenceApp.Models;
using System.IO;
using Xamarin.Forms;

using Xamarin.Forms.Xaml;

namespace ConferenceApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestionDetail : ContentPage
    {
        public QuestionDetail(Questions selected)
        {
            InitializeComponent();

            _UserEmail.Text = selected.UserEmail;
            _Question.Text = selected.Question;

          //  FlowDocument
        }
        
    }
}