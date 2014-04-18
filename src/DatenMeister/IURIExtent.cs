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
        /// <param name="type">Type of the object, may be null, if not known</param>
        /// <returns>Created object that is returned</returns>
        IObject CreateObject(IObject type = null);

        /// <summary>
        /// Removes the object from extent. 
        /// This is not part of MOF Standard
        /// </summary>
        /// <param name="element">Element to be removed</param>
        void RemoveObject(IObject element);

        /// <summary>
        /// Gets or sets the pool, where the extent is being assigned
        /// </summary>
        IPool Pool
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a flag whether the extent is currently dirty
        /// That means, it has unsaved changes
        /// </summary>
        bool IsDirty
        {
            get;
            set;
        }
    }
}
