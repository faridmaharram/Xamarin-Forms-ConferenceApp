using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceApp
{
    public interface ISave
    {
        Task SaveTextAsync(string filename, string contentType, MemoryStream s);
    }
}
