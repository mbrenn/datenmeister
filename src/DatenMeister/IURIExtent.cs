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
        IReflectiveSequence Elements();

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
