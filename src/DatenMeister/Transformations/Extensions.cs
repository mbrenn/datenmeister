using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations
{
    /// <summary>
    /// Defines the extension methods
    /// </summary>
    public static class Extensions
    {
        public static IReflectiveCollection Recurse(this IReflectiveCollection collection)
        {
            return new RecurseObjectTransformation(collection);
        }

        public static IReflectiveCollection FilterByType(this IReflectiveCollection collection, IObject type)
        {
            return new FilterByTypeTransformation(collection, type);
        }

        public static IReflectiveCollection FilterByType(this IReflectiveCollection collection, string typeName)
        {
            return new FilterByTypeTransformation(collection, typeName);
        }

        public static IReflectiveCollection FilterByProperty(this IReflectiveCollection collection, string propertyName, object value)
        {
            return new FilterByPropertyTransformation(collection, propertyName, value);
        }
    }
}
