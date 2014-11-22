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
        /// Initializes a new instance of the Workbench class.
        /// </summary>
        public Workbench()
        {
            this.instances = new List<ExtentInfo>();
        }

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

        /// <summary>
        /// Gets or sets a list of all loaded extents. Does not contain the instances themselves, but contains the loading
        /// and storing behavior. This information is necessary to reload the extent.
        /// </summary>
        public IList<ExtentInfo> instances
        {
            get;
            set;
        }
    }
}
