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
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetConsolidatedProperties(this IReflectiveCollection collection)
        {
            var result = new List<string>();

            foreach (var item in collection)
            {
                if (item is IObject)
                {
                    foreach (var pair in item.AsIObject().getAll())
                    {
                        var propertyName = pair.PropertyName;
                        if (!result.Contains(propertyName))
                        {
                            result.Add(propertyName);
                        }
                    }
                }
            }

            return result;
        }
    }
}
