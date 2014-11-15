using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.WPF.Controls
{
    /// <summary>
    /// Enumeration of all available edit mode
    /// </summary>
    public enum EditMode
    {
        /// <summary>
        /// Edit mode is new
        /// </summary>
        New,
        /// <summary>
        /// Edit mode is editing a currently existing element
        /// </summary>
        Edit,

        /// <summary>
        /// Shows a read-only view of the element
        /// </summary>
        Read
    }

    /// <summary>
    /// Enumeration of all available display modes for a certain view
    /// </summary>
    public enum DisplayMode
    {
        /// <summary>
        /// Table view being used
        /// </summary>
        Table, 

        /// <summary>
        /// Formview being used
        /// </summary>
        Form
    }
}
