using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Pool
{
    public static class Extensions
    {
        /// <summary>
        /// Gets the extent by url out of the datapool
        /// </summary>
        /// <param name="pool">Pool to be used</param>
        /// <param name="uri">Uri being requested</param>
        /// <returns></returns>
        public static IURIExtent GetExtentByUri(this IPool pool, string uri)
        {
            return pool.ExtentContainer
                .Where(x => x.Extent.ContextURI() == uri)
                .Select(x => x.Extent)
                .FirstOrDefault();
        }

        /// <summary>
        ///  Resolves the uri by <c>Resolve</c> and returns the elements if the returned object is an Extent
        /// </summary>
        /// <param name="url">Url being used</param>
        /// <returns>Enumeration of objects</returns>
        public static IEnumerable<IObject> ResolveAsObjects(this IPoolResolver resolver, string url)
        {
            var result = resolver.Resolve(url);
            if (result is IURIExtent)
            {
                return (result as IURIExtent).Elements().Select(x => x.AsIObject());
            }

            if (result is IEnumerable<IObject>)
            {
                return result as IEnumerable<IObject>;
            }

            if (result is IReflectiveCollection)
            {
                return ResolveByReflectiveCollection(result as IReflectiveCollection);
            }

            return null;
        }

        /// <summary>
        /// Resolves by reflective collection and returns an enumeration of IObject
        /// </summary>
        /// <param name="reflectiveCollection">Reflective Collection being used</param>
        /// <returns>Enumeration of IObject</returns>
        private static IEnumerable<IObject> ResolveByReflectiveCollection(IReflectiveCollection reflectiveCollection)
        {
            foreach (var item in reflectiveCollection)
            {
                yield return item.AsIObject();
            }
        }
    }
}
