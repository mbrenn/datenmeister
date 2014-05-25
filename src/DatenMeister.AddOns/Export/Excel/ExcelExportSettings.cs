using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Export.Excel
{
    public class ExcelExportSettings
    {
        /// <summary>
        /// Gets or sets the path for the export settings
        /// </summary>
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the information whether one Excel sheet shall be created for every
        /// type of if all elements shall be combined in one sheet. 
        /// For each found property, one column will be created. 
        /// </summary>
        public bool PerTypeOneSheet
        {
            get;
            set;
        }
    }
}
