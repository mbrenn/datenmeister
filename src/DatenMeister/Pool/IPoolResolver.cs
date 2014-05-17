using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Pool
{
    /// <summary>
    /// Defines the interface being used to resolve an object by path
    /// and to get a path by the object
    /// </summary>
    public interface IPoolResolver
    {
        /// <summary>
        /// Gets or sets the pool being associated. 
        /// </summary>
        IPool Pool
        {
            get;
            set;
        }

        /// <summary>
        /// Resolves an object or an extent by a url or a query
        /// </summary>
        /// <param name="url">Url to be used</param>
        /// <returns>Resolved extent or url</returns>
        object Resolve(string url, IObject context = null);

        /// <summary>
        /// Gets the resolve path for a certain object that can be used to resolve
        /// the object
        /// </summary>
        /// <param name="obj">Object whose resolve path is required</param>
        /// <returns>The resolvepath of the object</returns>
        string GetResolvePath(IObject obj, IObject context = null);
    }
}
