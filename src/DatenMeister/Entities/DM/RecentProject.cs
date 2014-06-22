using DatenMeister.Entities.UML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.DM
{
    public class RecentProject : NamedElement
    {
        /// <summary>
        /// Gets or sets the datapath for the recent project
        /// </summary>
        public string filePath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the datetime, when the entry has been created
        /// </summary>
        public DateTime created
        {
            get;
            set;
        }
    }
}
