using DatenMeister.Entities.DM;
using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Pool
{
    /// <summary>
    /// Defines the interface for the pool, which contains all 
    /// the containers
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// Gets a list of extent instances. 
        /// This method shall be thread-safe
        /// </summary>
        IEnumerable<ExtentContainer> ExtentContainer
        {
            get;
        }

        /// <summary>
        /// Adds the an extent to the datenmeister pool and defines
        /// the storage path and a name to the given pool
        /// </summary>
        /// <param name="extent">Extent to be added</param>
        /// <param name="storagePath">Path, where pool is stored</param>
        /// <param name="name">Name of the pool</param>
        /// <param name="extentType">Type of the extent being stored</param>
        ExtentInfo Add(IURIExtent extent, string storagePath, string name, ExtentType extentType);
    }
}
