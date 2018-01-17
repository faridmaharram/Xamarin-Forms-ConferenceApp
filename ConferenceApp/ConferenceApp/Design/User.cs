using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceApp.Design
{

    public class User
    {
        private static string uid;

        public static string Email
        {

            get
            {
                return uid;
            }
            set
            {
                uid = value;
            }

        }


        private User() { }
    }
}
