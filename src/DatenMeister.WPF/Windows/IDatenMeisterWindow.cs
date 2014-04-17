
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
        public Func<IURIExtent> ExtentFactory
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

    public interface IDatenMeisterWindow
    {
        /// <summary>
        /// Adds a tab for an extent to the window
        /// </summary>
        /// <param name="name">Name of the window</param>
        /// <param name="parameters">Parameters to be used</param>
        void AddExtent(string name, AddExtentParameters parameters);
    }
}
