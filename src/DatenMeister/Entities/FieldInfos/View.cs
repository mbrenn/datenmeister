using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.FieldInfos
{
    /// <summary>
    /// Defines the properties of the view
    /// </summary>
    public class View
    {
        /// <summary>
        /// Gets or sets the name of the view
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the uri that shall be shown
        /// </summary>
        public string extentUri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a list of fieldinfos
        /// </summary>
        public IList<object> fieldInfos
        {
            get;
            set;
        }

        /// <summary>
        /// True, if the view shall start in edit mode. 
        /// Is used for Formview
        /// </summary>
        public bool startInEditMode
        {
            get;
            set;
        }
    }
}
