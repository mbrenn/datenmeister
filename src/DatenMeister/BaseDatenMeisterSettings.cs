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
        public DatenMeisterPool Pool
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
    }
}
