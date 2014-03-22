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

        /// Converts the extent to json object
        /// </summary>
        /// <param name="extent">Extent to be converted</param>
        /// <returns>Converted object</returns>
        public static object ToJson(this IObject element, IURIExtent extent)
        {
            var result = new
            {
                id = element.Id,
                extentUri = extent.ContextURI(),
                values = element.ToFlatObject(extent)
            };

            return result;
        }

        /// <summary>
        /// Transforms the given object to a pure Json-Object that can be used for web interaction
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Converted object</returns>
        public static Dictionary<string, object> ToFlatObject(this IObject value, IURIExtent extent)
        {
            var result = new Dictionary<string, object>();

            foreach (var pair in value.getAll())
            {
                var pairValue = ConvertAsFlatObject(pair.Value, extent);

                result[pair.PropertyName] = pairValue;
            }

            return result;
        }

        private static object ConvertAsFlatObject(object pairValue, IURIExtent extent)
        {
            if (IsNative(pairValue))
            {
                return pairValue;
            }

            if (pairValue is IEnumerable)
            {
                var listElements = new List<object>();
                foreach (var enumerationValue in pairValue as IEnumerable)
                {
                    listElements.Add(ConvertAsFlatObject(enumerationValue, extent));
                }

                return listElements;
            }
            
            if (pairValue is IObject)
            {
                return ToJson(pairValue as IObject, extent);
            }

            return pairValue;
        }

        public static void SetAll(this IObject value, Dictionary<string, object> values)
        {
            foreach (var pair in values)
            {
                value.set(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// Stores a list of all primitive types that are supported by DatenMeister directly
        /// </summary>
        private static List<Type> primitiveTypes = new List<Type>();

        /// <summary>
        /// Checks, if the given object is a native object or if the objects needs to be converted
        /// </summary>
        /// <param name="checkObject">CheckObject to be checked</param>
        /// <returns>true, if object is not native</returns>
        public static bool IsNative(object checkObject)
        {
            // An empty object is null
            if (checkObject == null)
            {
                return true;
            }

            // Initializes list of primitiveTypes if necessary
            if (primitiveTypes.Count == 0)
            {
                lock (primitiveTypes)
                {
                    primitiveTypes.Clear();
                    primitiveTypes.Add(typeof(Boolean));
                    primitiveTypes.Add(typeof(Int16));
                    primitiveTypes.Add(typeof(Int32));
                    primitiveTypes.Add(typeof(Int64));
                    primitiveTypes.Add(typeof(Double));
                    primitiveTypes.Add(typeof(Single));
                    primitiveTypes.Add(typeof(String));
                    primitiveTypes.Add(typeof(DateTime));
                    primitiveTypes.Add(typeof(TimeSpan));
                }
            }

            // Checks, if type of given object is in the list above
            return primitiveTypes.Contains(checkObject.GetType());
        }
    }
}
