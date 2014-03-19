using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Entities.UML
{
    /// <summary>
    /// UML 2.4.1 Infrastructure 10.1.3
    /// Elements with names are instances of NamedElement. The name for a named element is optional. If specified, then any valid
    /// string, including the empty string, may be used.
    /// </summary>
    public class NamedElement
    {
        /// <summary>
        /// Gets or sets the name of the named element
        /// </summary>
        public string name
        {
            get;
            set;
        }
    }
}
