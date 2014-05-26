using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls
{
    /// <summary>
    /// Stores the arguments which are necessary, wen the user tries to open an object 
    /// in list view or any other view
    /// </summary>
    public class DetailOpenEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the object itself, which has been selected by the user
        /// </summary>
        public IObject Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the associated object
        /// </summary>
        public IURIExtent Extent
        {
            get;
            set;
        }
    }
}
