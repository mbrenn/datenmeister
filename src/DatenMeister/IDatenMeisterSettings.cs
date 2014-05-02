using DatenMeister.DataProvider.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DatenMeister
{
    public interface IDatenMeisterSettings
    {
        /// <summary>
        /// Gets or sets the datameister pool
        /// </summary>
        DatenMeisterPool Pool
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the project extent
        /// </summary>
        IURIExtent ProjectExtent
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the settings for the extent
        /// </summary>
        XmlSettings ExtentSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Creates an empty document being used
        /// </summary>
        /// <returns>Creates an empty document</returns>
        XDocument CreateEmpty(); 
    }
}
