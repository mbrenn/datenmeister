using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    /// <summary>
    /// Stores the extent information into the datapool and adds some additional information
    /// </summary>
    public class ExtentInfoForPool
    {
        /// <summary>
        /// Gets or sets the path, where the extent will be stored. 
        /// This may be a filename or a database connection string. The Path may be null, if it is just stored 
        /// in memory. 
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string StoragePath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the extent
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the extent
        /// </summary>
        public ExtentType ExtentType
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the ExtentInstance class. 
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        /// <param name="path">Path, where data will be stored</param>
        public ExtentInfoForPool(string path, ExtentType extentType)
        {
            this.StoragePath = path;
            this.ExtentType = extentType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatenMeister.PoolInstance"/> class.
        /// </summary>
        /// <param name='extent'>
        /// Extent.
        /// </param>
        /// <param name='storagePath'>
        /// Path.
        /// </param>
        public ExtentInfoForPool(string storagePath, string name, ExtentType extentType)
        {
            this.StoragePath = storagePath;
            this.Name = name;
            this.ExtentType = extentType;
        }
    }
}
