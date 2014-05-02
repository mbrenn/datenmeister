
using DatenMeister.DataProvider.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Windows
{
    public class AddExtentParameters
    {
        /// <summary>
        /// Gets or sets the function to be used to create the extent
        /// </summary>
        public Func<IURIExtent, IURIExtent> ExtentFactory
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object being used to define the view properties for the list view
        /// </summary>
        public IObject TableViewInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object being used to define the view properties for the detail view
        /// </summary>
        public IObject DetailViewInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the factory being used to create a new element out of the given extent
        /// </summary>
        public Func<IObject> ElementFactory
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Defines all the methods that are implemented by the DatenMeister
    /// </summary>
    public interface IDatenMeisterWindow
    {
        /// <summary>
        /// Gets or sets the datenmeister settings
        /// </summary>
        IDatenMeisterSettings DatenMeisterSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the title of the application
        /// </summary>
        /// <param name="title">Title of the application</param>
        void SetTitle(string title);

        /// <summary>
        /// Adds a tab for an extent to the window
        /// </summary>
        /// <param name="name">Name of the window</param>
        /// <param name="parameters">Parameters to be used</param>
        void AddExtent(string name, AddExtentParameters parameters);
    }
}
