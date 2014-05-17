using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Pool
{
    public static class Extensions
    {
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
