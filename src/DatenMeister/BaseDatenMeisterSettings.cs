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
                return PoolResolver.GetDefaultPool().GetExtent(ExtentType.Data).FirstOrDefault();
            }
        }

        public IURIExtent ViewExtent
        {
            get
            {
                return PoolResolver.GetDefaultPool().GetExtent(ExtentType.View).FirstOrDefault();
            }
        }

        /// <summary>
        /// Stores the meta extent being used to 
        /// </summary>
        public IURIExtent TypeExtent
        {
            get
            {
                return PoolResolver.GetDefaultPool().GetExtent(ExtentType.Type).LastOrDefault();
            }
        }

        public XmlSettings ExtentSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Creates an empty document
        /// </summary>
        /// <returns>The created document, which is queried, when user creates a new file</returns>
        public abstract XDocument CreateEmpty();
        
        /// <summary>
        /// Performs the full initialization at application start. 
        /// This method is just called once
        /// </summary>
        public void InitializeForAppStart();
        
        /// <summary>
        /// This function will be called, when a new viewset needs to be created. 
        /// It is independent to the fact whether the containing extents and viewinformation
        /// is loaded or is created from Scratch.
        /// </summary>
        public abstract void InitializeViewSet();

        /// <summary>
        /// The function will be called, when the user wants to have an extent/viewset from
        /// scratch. This means, that he has clicked "File->New"
        /// </summary>
        public abstract void CreateFromScratch();
        
        /// <summary>
        /// The function will be called, when application has been started. 
        /// It can be used to include some example data
        /// </summary>
        public abstract void CreateForApplicationStart();
    }
}
