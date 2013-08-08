using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.CSV
{
    public class CSVSettings
    {
        public CSVSettings()
        {
            this.Encoding = Encoding.UTF8;
            this.HasHeader = true;
            this.Separator = ",";    
        }

        public Encoding Encoding
        {
            get;
            set;
        }

        public bool HasHeader
        {
            get;
            set;
        }

        public string Separator
        {
            get;
            set;
        }
    }
}
