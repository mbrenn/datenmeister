using BurnSystems.Test;
using DatenMeister.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DatenMeister.Pool
{
    /// <summary>
    /// Resolves extents and extent objects by Uri
    /// </summary>
    public class PoolResolver
    {
        /// <summary>
        /// Stores the pool
        /// </summary>
        private DatenMeisterPool pool;

        /// <summary>
        /// Initializes a new instance of DatenMeisterPool class
        /// </summary>
        /// <param name="pool"></param>
        public PoolResolver(DatenMeisterPool pool)
        {
            this.pool = pool;
        }

        /// <summary>
        /// Initializes a new instance of DatenMeisterPool class
        /// </summary>
        /// <param name="pool">Pool</param>
        public PoolResolver(IPool pool)
        {
            this.pool = pool as DatenMeisterPool;
            Ensure.That(this.pool != null);
        }

        /// <summary>
        /// Resolves an object or an extent by a url or a query
        /// </summary>
        /// <param name="url">Url to be used</param>
        /// <returns>Resolved extent or url</returns>
        public object Resolve(string url)
        {
            var uri = new Uri(url);

            //
            // First, retrieve the extent matching uri
            // 
            // Now try to find url
            var extentUrl = new Uri(uri.Scheme + "://" + uri.Authority + uri.AbsolutePath);
            var extent = this.pool.Extents.Where(x =>
                {
                    return new Uri(x.ContextURI()) == extentUrl;
                }).FirstOrDefault();
            if (extent == null)
            {
                // No uri had been found
                return null;
            }

            // Checks, if we have a query identifier
            if (!string.IsNullOrEmpty(uri.Query))
            {
                var dictQuery = HttpUtility.ParseQueryString(uri.Query);
                var requestedType = dictQuery["type"];

                if (!string.IsNullOrEmpty(requestedType))
                {
                    extent = extent.FilterByType(requestedType);
                }
            }

            // Gets the fragment to identify the object
            var fragment = uri.Fragment;
            if (string.IsNullOrEmpty(fragment))
            {
                // We don't have a fragment, so return the extent
                return extent;
            }
            else
            {
                if (fragment.StartsWith("#"))
                {
                    fragment = fragment.Substring(1);
                }

                // We got a fragment, find the element with the id
                return extent.Elements().Where(x => x.Id == fragment).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the resolve path for a certain object that can be used to resolve
        /// the object
        /// </summary>
        /// <param name="obj">Object whose resolve path is required</param>
        /// <returns>The resolvepath of the object</returns>
        public string GetResolvePath(IObject obj)
        {
            Ensure.That(obj.Extent != null, "GetResolvePath: Given object has no extent");
            Ensure.That(obj.Extent.Pool != null, "GetResolvePath: Given object is attached to an extent without connected pool");

            var result = string.Format("{0}#{1}", obj.Extent.ContextURI(), obj.Id);
#if DEBUG
            var backCheck=Resolve(result) as IObject ;

            Ensure.That(backCheck != null, "GetResolvePath returned an unresolvable object: " + result);
            Ensure.That(backCheck.Id == obj.Id, "GetResolvePath returned wrong object: " + result);
#endif
            return result;
        }

        /// <summary>
        ///  Resolves the uri by <c>Resolve</c> and returns the elements if the returned object is an Extent
        /// </summary>
        /// <param name="url">Url being used</param>
        /// <returns>Enumeration of objects</returns>
        public IEnumerable<IObject> ResolveAsObjects(string url)
        {
            var result = this.Resolve(url);
            if (result is IURIExtent)
            {
                return (result as IURIExtent).Elements();
            }

            if (result is IEnumerable<IObject>)
            {
                return result as IEnumerable<IObject>;
            }

            return null;
        }
    }
}
