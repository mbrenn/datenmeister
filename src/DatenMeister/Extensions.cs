using DatenMeister.DataProvider;
using DatenMeister.Pool;
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
                var resolved = asResolvable.Resolve(pool, context);
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
                var poolResolver = PoolResolver.GetDefault(pool);
                return poolResolver.Resolve(objAsString, context);
            }

            throw new NotImplementedException("Type of resolve cannot be done...");
        }

        public static JsonExtentInfo ToJson(this IURIExtent extent)
        {
            return new JsonExtentInfo()
            {
                uri = extent.ContextURI(),
                type = extent.GetType().FullName
            };
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
            if (value is string)
            {
                // If value is a string, return the complete string, not just the first letter
                return value;
            }

            // Ok, we have an unspecified thing... don't like, but necessary
            var valueAsUnspecified = value as IUnspecified;
            if (valueAsUnspecified != null)
            {
                return valueAsUnspecified.AsSingle();
            }

            // Checks, if we have an enumeration, if yes, return first element
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
                foreach (var element in valueAsEnumeration)
                {
                    yield return element;
                }
            }
            else
            {
                yield return value;
            }
        }

        public static IReflectiveCollection AsReflectiveCollection(this object value)
        {
            // Ok, we have an unspecified thing... don't like, but necessary
            var valueAsUnspecified = value as IUnspecified;
            if (valueAsUnspecified != null)
            {
                return valueAsUnspecified.AsReflectiveCollection();
            }

            throw new NotImplementedException("Only instances implemented IUnspecified can be transformed to a reflective collection");
        }

        public static IReflectiveSequence AsReflectiveSequence(this object value)
        {

            // Ok, we have an unspecified thing... don't like, but necessary
            var valueAsUnspecified = value as IUnspecified;
            if (valueAsUnspecified != null)
            {
                return valueAsUnspecified.AsReflectiveSequence();
            }

            throw new NotImplementedException("Only instances implemented IUnspecified can be transformed to a reflective sequence");
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

            throw new InvalidOperationException("Given object is not of type DatenMeister.IObject");
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

        /// <summary>
        /// Returns the information whether the given object might be true or false
        /// </summary>
        /// <param name="value">Object to be tested</param>
        public static bool ToBoolean(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is bool)
            {
                return (bool)value;
            }

            if (value is string)
            {
                return value.ToString() == "True" || value.ToString() == "true";
            }

            if (value is int)
            {
                return ((int)value) != 0;
            }

            return false;
        }
    }
}
