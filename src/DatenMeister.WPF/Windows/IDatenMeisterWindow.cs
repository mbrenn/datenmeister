
using DatenMeister.DataProvider.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Defines all the methods that are implemented by the DatenMeister
    /// </summary>
    public interface IDatenMeisterWindow
    {
        /// <summary>
        /// Gets or sets the datenmeister settings
        /// </summary>
        IDatenMeisterSettings Settings
        {
            get;
            set;
        }

        /// <summary>
        /// Refreshes the views
        /// </summary>
        void RefreshViews();

        /// <summary>
        /// Sets the title of the application
        /// </summary>
        /// <param name="title">Title of the application</param>
        void SetTitle(string title);

        /// <summary>
        /// Adds a menuentry to the application window
        /// </summary>
        /// <param name="menuHeadline">Headline of the menu</param>
        /// <param name="menuLine">Menu that shall be added</param>
        /// <param name="press"></param>
        void AddMenuEntry(string menuHeadline, MenuItem menuLine);
    }
}
