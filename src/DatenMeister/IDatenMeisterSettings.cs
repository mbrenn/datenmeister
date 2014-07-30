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
        /// Gets or sets the applicationname
        /// </summary>
        string ApplicationName
        {
            get;
            set;
        } 
        
        /// <summary>
        /// Gets or sets te window title
        /// </summary>
        string WindowTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the datameister pool
        /// </summary>
        IPool Pool
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
        /// Gets or sets the extent that is used to find the main views
        /// </summary>
        IURIExtent ViewExtent
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the meta extent being used to 
        /// </summary>
        IURIExtent TypeExtent
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
