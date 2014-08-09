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
        /// Gets or sets the project extent
        /// </summary>
        IURIExtent ProjectExtent
        {
            get;
        }

        /// <summary>
        /// Gets or sets the extent that is used to find the main views
        /// </summary>
        IURIExtent ViewExtent
        {
            get;
        }

        /// <summary>
        /// Stores the meta extent being used to 
        /// </summary>
        IURIExtent TypeExtent
        {
            get;
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

        /// <summary>
        /// Performs the full initialization at application start. 
        /// This method is just called once
        /// </summary>
        void InitializeForAppStart();

        /// <summary>
        /// This function will be called, when a new viewset needs to be created. 
        /// It is independent to the fact whether the containing extents and viewinformation
        /// is loaded or is created from Scratch.
        /// </summary>
        void InitializeViewSet();

        /// <summary>
        /// The function will be called, when the user wants to have an extent/viewset from
        /// scratch. This means, that he has clicked "File->New"
        /// </summary>
        void CreateFromScratch();

        /// <summary>
        /// The function will be called, when application has been started. 
        /// It can be used to include some example data
        /// </summary>
        void CreateForApplicationStart();
    }
}
