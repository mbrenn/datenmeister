using DatenMeister.DataProvider;
using DatenMeister.Entities.DM;
using DatenMeister.Pool;
using Ninject;
using System;
using System.Collections;
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
        public static IEnumerable<ExtentInfo> GetInstances(this IPool logic)
        {
            return logic.ExtentContainer.Select(x => x.Info);
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
        public static ExtentInfo GetInstance(this IPool pool, string extentUri)
        {
            return pool.ExtentContainer.Where(x => x.Extent.ContextURI() == extentUri).Select(x => x.Info).FirstOrDefault();
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

        /// <summary>
        /// Resolves the object if necessary. 
        /// If there is no reason to resolve the object, the object itself is returned
        /// </summary>
        /// <param name="pool">Pool, which shall be used to resolve the object</param>
        /// <param name="obj">Object that is used for resolvinb</param>
        /// <returns>Returned object that can be used</returns>
        public static object Resolve(this IPool pool, IObject context, object obj)
        {
            var asEnumerable = obj as IEnumerable;
            if (asEnumerable != null && !(obj is string))
            {
                var list = new List<object>();
                foreach (var value in asEnumerable)
                {
                    // Not very beautiful. It would be better to make this more abstract and to resolve
                    // the values within the list itself
                    list.Add(Resolve(pool, context, value));
                }

                return list;
            }

            var asResolvable = obj as IResolvable;
            if (asResolvable != null)
            {
                // Resolve object and see, if it can be further resolved
                var resolved = asResolvable.Resolve();
                return Resolve(pool, context, resolved);
            }

            return obj;
        }

        /// <summary>
        /// Resolves the given object by using the PoolResolver. 
        /// It resolves object by path or other means
        /// </summary>
        /// <param name="pool">Pool being used to resolve</param>
        /// <param name="obj">Object to be resolved</param>
        /// <returns>Object being resolved</returns>
        public static object ResolveByPath(this IPool pool, string obj, IObject context = null)
        {
            // at the moment, nothing to resolve
            var objAsString = obj as String;

            if (objAsString != null)
            {
                var poolResolver = Injection.Application.Get<IPoolResolver>();
                return poolResolver.Resolve(objAsString, context);
            }

            throw new NotImplementedException("Type of resolve cannot be done...");
        }

        /// <summary>
        /// Gets the enumeration of extent types
        /// </summary>
        /// <param name="pool">Pool to be queried</param>
        /// <param name="extentType">The extent type to be queried</param>
        /// <returns>Enumeration, matching to the given extentType</returns>
        public static IEnumerable<ExtentInfo> Get(this IPool pool, ExtentType extentType)
        {
            return pool.ExtentContainer.Where(x => x.Info.extentType == extentType).Select(x => x.Info);
        }

        /// <summary>
        /// Gets the enumeration of extent types
        /// </summary>
        /// <param name="pool">Pool to be queried</param>
        /// <param name="extentType">The extent type to be queried</param>
        /// <returns>Enumeration, matching to the given extentType</returns>
        public static IEnumerable<IURIExtent> GetExtent(this IPool pool, ExtentType extentType)
        {
            return pool.ExtentContainer.Where(x => x.Info.extentType == extentType).Select(x => x.Extent);
        }

        /// <summary>
        /// Gets the extent instance of a certain extent
        /// </summary>
        /// <param name="pool">Pool to be used</param>
        /// <param name="extent">Extent whose instance is queried</param>
        /// <returns>Found extent instance</returns>
        public static ExtentInfo GetInstance(this IPool pool, IURIExtent extent)
        {
            return pool.ExtentContainer.Where(x => x.Extent == extent).Select(x => x.Info).FirstOrDefault();
        }
    }
}
