
using BurnSystems.Test;
using DatenMeister;
using DatenMeister.Logic;
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
        /// Gets or sets the pool, which shall be used for resolving
        /// </summary>
        public IPool Pool
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the context which shall be used for resolving
        /// </summary>
        public IObject Context
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the resolvable by path instance
        /// </summary>
        /// <param name="path">Path to be resolved</param>
        public ResolvableByPath(IPool pool, IObject context, string path)
        {
            Ensure.That(pool != null);

            this.Pool = pool;
            this.Context = context;
            this.Path = path;
        }

        /// <summary>
        /// Resolves the given object
        /// </summary>
        /// <param name="pool">Pool being used to resolve by path</param>
        /// <returns>Object to be resolved</returns>
        public object Resolve()
        {
            return this.Pool.ResolveByPath(this.Path, this.Context);
        }
    }
}
