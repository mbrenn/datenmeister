using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    public interface IURIExtent
    {
        string ContextURI();

        /// <summary>
        /// Gets all elements of the extent
        /// </summary>
        /// <returns>Enumeration of extents</returns>
        IEnumerable<IObject> Elements();

        /// <summary>
        /// Creates an empty object that already had been assigned to extent
        /// </summary>
        /// <returns>Created object that is returned</returns>
        IObject CreateObject();

        /// <summary>
        /// Removes the object from extent. 
        /// This is not part of MOF Standard
        /// </summary>
        /// <param name="element">Element to be removed</param>
        void RemoveObject(IObject element);
    }
}
