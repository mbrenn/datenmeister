using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister
{
    /// <summary>
    /// MOF 2.4.1 Chapter 9.1
    /// Returns the parent container of this element if any. Return Null if there is no containing element.
    /// </summary>
    public interface IElement : IObject
    {
        /// <summary>
        /// Returns the Class that describes this element.
        /// </summary>
        /// <returns>Element containing the class information</returns>
        IObject getMetaClass();

        /// <summary>
        /// Returns the parent container of this element if any. Return Null if there is no containing element.
        /// </summary>
        /// <returns></returns>
        IObject container();
    }
}
