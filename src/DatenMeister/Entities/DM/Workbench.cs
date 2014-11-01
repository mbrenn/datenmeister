using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.DM
{
    public class Workbench
    {
        /// <summary>
        /// Gets or sets the file path, where the workbench is stored
        /// </summary>
        public string path
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the workbench. The type is used for initialisation of the complete workbench
        /// </summary>
        public string type
        {
            get;
            set;
        }

        public IList<ExtentInstance> Instances
        {
            get;
            set;
        }
    }
}
