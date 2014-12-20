
using DatenMeister.DataProvider.Xml;
using DatenMeister.Logic;
using DatenMeister.Logic.Settings;
using DatenMeister.WPF.Controls;
using DatenMeister.WPF.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        ApplicationCore Core
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the settings
        /// </summary>
        IPublicDatenMeisterSettings Settings
        {
            get;
        }

        /// <summary>
        /// Refreshes the views
        /// </summary>
        void RefreshTabs();

        /// <summary>
        /// Adds a menuentry to the application window
        /// </summary>
        /// <param name="menuHeadline">Headline of the menu</param>
        /// <param name="menuLine">Menu that shall be added</param>
        /// <param name="press"></param>
        void AddMenuEntry(string menuHeadline, UIElement menuLine);

        /// <summary>
        /// Associates a user-defined event, when the user doubleclicks on a detail field
        /// </summary>
        /// <param name="view">View, to which the detailopen event shall be associated</param>
        /// <param name="action">Action, which shall be executed instead of opening the detail dialog</param>
        void AssociateDetailOpenEvent(IObject view, Action<DetailOpenEventArgs> action);

        /// <summary>
        /// Loads and opens a file and refreshes the window, so recently loaded extent is included
        /// </summary>
        /// <param name="path">Path of the object to be loaded</param>
        void LoadAndOpenFile(string path);
    }
}
