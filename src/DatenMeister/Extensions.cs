using DatenMeister.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    public static class Extensions
    {
        public static JsonExtentInfo ToJson(this IURIExtent extent)
        {
            return new JsonExtentInfo()
            {
                uri = extent.ContextURI(),
                type = extent.GetType().FullName
            };
        }

        /// <summary>
        /// Converts the object a list object. 
        /// If this is already a list, the list will be returned, otherwise the object will be put into a list
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>Enumeration of objects</returns>
        public static IEnumerable<object> AsEnumeration(this object value)
        {
            var valueAsEnumeration = value as IEnumerable;
            if (valueAsEnumeration != null)
            {
                foreach ( var element in valueAsEnumeration)
                {
                    yield return element;
                }
            }
            else
            {
                yield return value;
            }
        }

        /// <summary>
        /// If the given object is an enumeration, it returns the first instance of the given object.
        /// Otherwise it returns the object itself. 
        /// If the object is null or enumeration is empty, null will be returned
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Converted object</returns>
        public static object AsSingle(this object value)
        {
            var valueAsEnumeration = value as IEnumerable;
            if (valueAsEnumeration != null)
            {
                return valueAsEnumeration.OfType<object>().FirstOrDefault();
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Gets the given object as an IObject interface
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>Returns given object, if it is an IObject</returns>
        public static IObject AsIObject(this object value)
        {
            var asObject = (value.AsSingle()) as IObject;
            if (asObject != null)
            {
                return asObject;
            }

            throw new InvalidOperationException("Given object is not a valid object");
        }
    }
}
