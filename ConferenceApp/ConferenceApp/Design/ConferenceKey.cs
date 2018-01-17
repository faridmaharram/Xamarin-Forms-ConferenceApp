using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceApp.Design
{
    public class ConferenceKey
    {
        private static string Confid;

        public static string ConKey
        {

            get
            {
                return Confid;
            }
            set
            {
                Confid = value;
            }

        }


        private ConferenceKey() { }
    }
}
