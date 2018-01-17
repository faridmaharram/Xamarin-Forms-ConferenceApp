using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceApp.Design
{
     public class QuestionKey
    {
        private static string Queid;

        public static string QueKey
        {

            get
            {
                return Queid;
            }
            set
            {
                Queid = value;
            }

        }


        private QuestionKey() { }
    }
}
