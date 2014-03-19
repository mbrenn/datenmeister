using System;
namespace DatenMeister.Logic
{
    /// <summary>
    /// Stores the datapool and some additional information
    /// </summary>
    public class ExtentInstance
    {
        /// <summary>
        /// Gets or sets the extent.
        /// </summary>
        /// <value>
        /// The extent.
        /// </value>
        public IURIExtent Extent
        {
            get;
            set;
        }

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
        /// Initializes a new instance of the ExtentInstance class. 
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        /// <param name="path">Path, where data will be stored</param>
        public ExtentInstance(IURIExtent extent, string path)
        {
            this.Extent = extent;
            this.StoragePath = path;
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
        public ExtentInstance(IURIExtent extent, string storagePath, string name)
        {
            this.Extent = extent;
            this.StoragePath = storagePath;
            this.Name = name;
        }

        /// <summary>
        /// Converts the instance to an object which can be sent to browser
        /// </summary>
        /// <returns>Result to be returned</returns>
        public object ToJson()
        {
            return new
            {
                name = this.Name,
                uri = this.Extent.ContextURI(),
                type = this.Extent.GetType().Name,
                filename = this.StoragePath
            };
        }
    }
}

