
using DatenMeister;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.Common
{
    /// <summary>
    /// Resolves the object that is resolvable by path
    /// </summary>
    public class ResolvableByPath : IResolvable
    {
        /// <summary>
        /// Gets or sets the path that shall be resolved
        /// </summary>
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the resolvable by path instance
        /// </summary>
        /// <param name="path">Path to be resolved</param>
        public ResolvableByPath(string path)
        {
            this.Path = path;
        }

        /// <summary>
        /// Resolves the given object
        /// </summary>
        /// <param name="pool">Pool being used to resolve by path</param>
        /// <returns>Object to be resolved</returns>
        public object Resolve(IPool pool, IObject context)
        {
            return pool.ResolveByPath(this.Path, context);
        }
    }
}
