using System;
namespace DatenMeister.Logic
{
    /// <summary>
    /// Defines the possible extent types
    /// </summary>
    public enum ExtentType
    {
        /// <summary>
        /// Defines that the given extent contains all extents
        /// </summary>
        Extents, 

        /// <summary>
        /// Defines the types necessary to execute the application itself
        /// </summary>
        MetaType,
        
        /// <summary>
        /// Types of the application itself
        /// </summary>
        Type,

        /// <summary>
        /// Views of the application
        /// </summary>
        View, 

        /// <summary>
        /// Data of the current project
        /// </summary>
        Data,
        
        /// <summary>
        /// Data for the application being used to 
        /// </summary>
        ApplicationData,

        /// <summary>
        /// Queries which contain a filtered, sorted or any other type of extent being dependent on one
        /// of the extents above
        /// </summary>
        Query
    }

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
        public ExtentInstance(IURIExtent extent, string path, ExtentType extentType)
        {
            this.Extent = extent;
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
        public ExtentInstance(IURIExtent extent, string storagePath, string name, ExtentType extentType)
        {
            this.Extent = extent;
            this.StoragePath = storagePath;
            this.Name = name;
            this.ExtentType = extentType;
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
                filename = this.StoragePath,
                isDirty = this.Extent.IsDirty, 
                extentType = this.ExtentType.ToString()
            };
        }

        public override string ToString()
        {
            if (this.Extent != null)
            {
                return this.Extent.ContextURI() + " (" + this.ExtentType.ToString() + ")";
            }
            else
            {
                return "ExtentInstance without Extent  (" + this.ExtentType.ToString() + ")";
            }
        }
    }
}

