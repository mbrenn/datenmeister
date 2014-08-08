using DatenMeister.DataProvider.Xml;
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

        /// <summary>
        /// Gets or sets the datameister pool
        /// </summary>
        public IPool Pool
        {
            get;
            set;
        }

        public IURIExtent ProjectExtent
        {
            get;
            set;
        }

        public IURIExtent ViewExtent
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the meta extent being used to 
        /// </summary>
        public IURIExtent TypeExtent
        {
            get;
            set;
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
        /// This function will be called, when a new viewset needs to be created. 
        /// It is independent to the fact whether the containing extents and viewinformation
        /// is loaded or is created from Scratch.
        /// </summary>
        public abstract void InitializeViewSet(IPool pool);

        /// <summary>
        /// The function will be called, when the user wants to have an extent/viewset from
        /// scratch. This means, that he has clicked "File->New"
        /// </summary>
        public abstract void CreateFromScratch(IPool pool);
    }
}
