using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    /// <summary>
    /// This class offers some helper methods for a reflective sequence
    /// </summary>
    public static class ReflectiveSequenceHelper
    {
        /// <summary>
        /// Gets a list of all properties, that are at least available in one properties. 
        /// Can also be seen as Max properties
        /// </summary>
        /// <param name="collection">Collection, whose properties shall get reflected</param>
        /// <returns>Enumeration of property names being used</returns>
        public static IEnumerable<string> GetConsolidatedPropertyNames(this IReflectiveCollection collection)
        {
            return ObjectHelper.GetColumnNames(collection.Where(x=> x is IObject).Select(x=> x as IObject));
        }
    }
}
