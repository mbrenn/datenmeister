using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
	/// <summary>
	/// Implements a some of helper methods for the pools
	/// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets a list of extent information. 
        /// This method shall be thread-safe
        /// </summary>
        public static IEnumerable<ExtentInfoForPool> GetInstances(this IPool logic)
        {
            return logic.ExtentContainer.Select(x => x.ExtentInfo);
        }

        /// <summary>
        /// Gets a list of extents
        /// This method shall be thread-safe
        /// </summary>
        public static IEnumerable<IURIExtent> GetExtents(this IPool logic)
        {
            return logic.ExtentContainer.Select(x => x.Extent);
        }

        /// <summary>
        /// Gets the instance within the pool by the extent Uri
        /// </summary>
        /// <param name="pool">Pool to be queried</param>
        /// <param name="extentUri">Uri of the extent</param>
        /// <returns>Found extent id</returns>
        public static ExtentInfoForPool GetInstance(this IPool pool, string extentUri)
        {
            return pool.ExtentContainer.Where(x => x.Extent.ContextURI() == extentUri).Select(x => x.ExtentInfo).FirstOrDefault();
        }

        /// <summary>
        /// Gets the container within the pool by the extent Uri
        /// </summary>
        /// <param name="pool">Pool to be queried</param>
        /// <param name="extentUri">Uri of the extent</param>
        /// <returns>Found extent id</returns>
        public static ExtentContainer GetContainer(this IPool pool, string extentUri)
        {
            return pool.ExtentContainer.Where(x => x.Extent.ContextURI() == extentUri).FirstOrDefault();
        }
    }
}
