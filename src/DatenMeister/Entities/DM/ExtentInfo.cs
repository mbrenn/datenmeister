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
    public class ExtentInfo
    {
        /// <summary>
        /// Gets or sets the url, under which the extent can be found
        /// The url has to be unique for all instances.
        /// </summary>
        public string uri
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
        /// Gets or sets the path, where the extent itself is stored
        /// </summary>
        public string storagePath
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

        /// <summary>
        /// Gets or sets the configuration being used to load the extent. It also includes the type of extent (.Net, Xml, etc) by using the correct metaClass
        /// </summary>
        public object loadConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the extent itself
        /// </summary>
        public string extentClass
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the ExtentInWorkbench class
        /// </summary>
        public ExtentInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ExtentInfo class.
        /// </summary>
        /// <param name='extent'>
        /// Extent.
        /// </param>
        /// <param name='storagePath'>
        /// Path.
        /// </param>
        public ExtentInfo(string storagePath, string name, ExtentType extentType, string uri, string extentClass)
        {
            this.storagePath = storagePath;
            this.name = name;
            this.extentType = extentType;
            this.uri = uri;
            this.extentClass = extentClass;
        }
    }
}
