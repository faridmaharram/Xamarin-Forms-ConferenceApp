using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceApp.Models
{
    public class Conference
    {


        public string Key { get; set; }
        public string CreaterName { get; set; }
        public string CreaterSurname { get; set; }
        public string CreatorEmail { get; set; }
        public string ConferenceName { get; set; }
        public string ConferenceCode { get; set; }
        public bool Status { get; set; }
    }
}
