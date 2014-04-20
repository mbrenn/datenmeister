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
    }
}
