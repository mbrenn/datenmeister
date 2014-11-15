using DatenMeister.Entities.UML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.FieldInfos
{
    /// <summary>
    /// Defines the tree view
    /// </summary>
    public class TreeView : NamedElement
    {
        /// <summary>
        /// Gets or sets the uri that shall be shown
        /// </summary>
        public string extentUri
        {
            get;
            set;
        }
    }
}
