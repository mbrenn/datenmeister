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

        private static int GetTypeCount(IReflectiveCollection collection)
        {
            return collection.Where(x => x is IElement)         // Need meta classes
                .Select(x => (x as IElement).getMetaClass())    // As meta classed
                .Where(x => x != null)                          // Which are metaclasses
                .Distinct()                                     // Grouped
                .Count();                                       // As Count
        }

        private static int GetExtentCount(IReflectiveCollection collection)
        {
            return collection.Where(x => x is IObject)          // Need IObjects
                .Select(x => (x as IObject).Extent)             // given me their extent
                .Where(x => x != null)                          // which are not null
                .Distinct()                                     // Grouped
                .Count();                                       // As Count
        }
        
        /// <summary>
        /// Gets the consolidated information about the complete ReflectiveCollection
        /// </summary>
        /// <param name="collection">Collection, whose properties shall get reflected</param>
        /// <returns>Enumeration of property names being used</returns>
        public static TypeInformation GetConsolidatedInformation(this IReflectiveCollection collection)
        {
            var result = new TypeInformation()
            {
                PropertyNames = GetConsolidatedPropertyNames(collection),
                TypeCount = GetTypeCount(collection),
                ExtentCount = GetExtentCount(collection)
            };

            return result;
        }

        public class TypeInformation
        {
            /// <summary>
            /// Gets a list of all properties, that are at least available in one properties. 
            /// </summary>
            public IEnumerable<string> PropertyNames
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the number of existing types in the collection under scope
            /// </summary>
            public int TypeCount
            {
                get;
                set;
            }

            public int ExtentCount
            {
                get;
                set;
            }
        }
    }
}
