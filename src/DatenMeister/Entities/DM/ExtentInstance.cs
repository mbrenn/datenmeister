using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.DM
{
    /// <summary>
    /// Gets or sets the information for the extent information
    /// </summary>
    public class ExtentInstance
    {
        /// <summary>
        /// Gets or sets the url, under which the extent can be found
        /// The url has to be unique for all instances
        /// </summary>
        public string url
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the extent type. The extent type defines the classifier of the
        /// Extent. 
        /// </summary>
        public ExtentType extentType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value whether the extent is prepopulated. 
        /// If the extent is prepopulated, it will not be loaded after the workbench has been loaded. 
        /// Prepopulated extents need to be filled by the workbench factory. 
        /// </summary>
        public bool isPrepopulated
        {
            get;
            set;
        }
    }
}
