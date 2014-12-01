using DatenMeister.DataProvider;
using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Pool
{
    /// <summary>
    /// Defines the parameter that will be used to add an extent
    /// </summary>
    public class ExtentParam
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
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
        /// Gets or sets the storage path
        /// </summary>
        public string StoragePath
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value whetner the extent was prepopulated
        /// </summary>
        public bool IsPrepopulated
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the settings for the data provider
        /// </summary>
        public ISettings DataProviderSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes  new instance of the ExtentParam class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="extentType"></param>
        /// <param name="storagePath"></param>
        public ExtentParam(string name, ExtentType extentType, string storagePath = null)
        {
            this.Name = name;
            this.ExtentType = extentType;
            this.StoragePath = storagePath;
        }

        /// <summary>
        /// Sets the property IsPrepopulated as true and returns the instance itself.
        /// </summary>
        /// <returns>The instance itself</returns>
        public ExtentParam AsPrepopulated()
        {
            this.IsPrepopulated = true;
            return this;
        }

        /// <summary>
        /// Sets the data provider setttings and returns the instance itself
        /// </summary>
        /// <param name="settings">Settings to be used</param>
        /// <returns>The instance itself</returns>
        public ExtentParam WithDataProviderSettings(ISettings settings)
        {
            this.DataProviderSettings = settings;
            return this;
        }
    }
}
