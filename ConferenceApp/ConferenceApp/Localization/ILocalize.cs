using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceApp.Localization
{
  public  interface ILocalize
    {
        void SetLocale();
        CultureInfo GetCurrentCultureInfo();

    }
}
