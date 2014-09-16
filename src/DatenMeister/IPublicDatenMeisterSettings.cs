using DatenMeister.DataProvider.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    /// <summary>
    /// Defines the settings, which are accessible for every application.
    /// </summary>
    public interface IPublicDatenMeisterSettings
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
        /// Stores the settings for the extent
        /// </summary>
        XmlSettings ExtentSettings
        {
            get;
            set;
        }
    }
}
