using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic;
using DatenMeister.Pool;
using DatenMeister.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DatenMeister
{
    public abstract class BaseDatenMeisterSettings : IDatenMeisterSettings
    {
        /// <summary>
        /// Gets or sets the application name
        /// </summary>
        public string ApplicationName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the window title
        /// </summary>
        public string WindowTitle
        {
            get;
            set;
        }

        public IURIExtent ProjectExtent
        {
            get
            {
                return PoolResolver.GetDefaultPool().GetExtents(ExtentType.Data).FirstOrDefault();
            }
        }

        public IURIExtent ViewExtent
        {
            get
            {
                return PoolResolver.GetDefaultPool().GetExtents(ExtentType.View).FirstOrDefault();
            }
        }

        /// <summary>
        /// Stores the meta extent being used to 
        /// </summary>
        public IURIExtent TypeExtent
        {
            get
            {
                return PoolResolver.GetDefaultPool().GetExtents(ExtentType.Type).LastOrDefault();
            }
        }

        public XmlSettings ExtentSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the BaseDatenMeisterSettings class.
        /// </summary>
        public BaseDatenMeisterSettings()
        {
            this.ExtentSettings = new XmlSettings();
        }
        
        /// <summary>
        /// Performs the full initialization at application start. 
        /// This method is just called once
        /// </summary>
        public abstract void InitializeForBootUp(ApplicationCore core);
        
        /// <summary>
        /// This function will be called, when a new viewset needs to be created. 
        /// It is independent to the fact whether the containing extents and viewinformation
        /// is loaded or is created from Scratch.
        /// </summary>
        public abstract void InitializeViewSet(ApplicationCore core);

        /// <summary>
        /// The function will be called, when the user wants to have an extent/viewset from
        /// scratch. This means, that he has clicked "File->New".
        /// It is recommended to create a complete new pool. 
        /// </summary>
        public abstract void InitializeFromScratch(ApplicationCore core);

        /// <summary>
        /// The function will be called, when the user has loaded a ViewSet. 
        /// </summary>
        public abstract void InitializeAfterLoading(ApplicationCore core);
        
        /// <summary>
        /// The function will be called, when application has been started. 
        /// It can be used to include some example data
        /// </summary>
        public abstract void InitializeForExampleData(ApplicationCore core);

        /// <summary>
        /// The function will be called, before the application gets closed.
        /// It can be used to store the latest changes. 
        /// </summary>
        /// <param name="core">Core to be used</param>
        public abstract void StoreViewSet(ApplicationCore core);
    }
}
