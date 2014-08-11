using DatenMeister.DataProvider;
using DatenMeister.Logic;
using DatenMeister.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
                var poolResolver = PoolResolver.GetDefault(pool);
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
        public static IEnumerable<ExtentInstance> Get(this IPool pool, ExtentType extentType)
        {
            return pool.Instances.Where(x => x.ExtentType == extentType);
        }

        /// <summary>
        /// Gets the enumeration of extent types
        /// </summary>
        /// <param name="pool">Pool to be queried</param>
        /// <param name="extentType">The extent type to be queried</param>
        /// <returns>Enumeration, matching to the given extentType</returns>
        public static IEnumerable<IURIExtent> GetExtent(this IPool pool, ExtentType extentType)
        {
            return pool.Instances.Where(x => x.ExtentType == extentType).Select(x => x.Extent);
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
        public static object AsSingle(this object value, bool fullResolve = true)
        {
            var valueAsResolvable = value as IResolvable;
            if (valueAsResolvable != null && fullResolve)
            {
                var result = valueAsResolvable.Resolve();
                if (fullResolve)
                {
                    return AsSingle(result, fullResolve);
                }

                return result;
            }

            if (value is string)
            {
                // If value is a string, return the complete string, not just the first letter
                // Without the condition, the string would be considered as an enumeration
                return value;
            }

            // Ok, we have an unspecified thing... don't like, but necessary
            var valueAsUnspecified = value as IUnspecified;
            if (valueAsUnspecified != null)
            {
                var asSingle = valueAsUnspecified.AsSingle();

                // Check, if the returned object is of type resolvable , IUnspecified or even null
                if (fullResolve)
                {
                    return Extensions.AsSingle(asSingle, fullResolve);
                }
                else
                {
                    return asSingle;
                }
            }

            // Checks, if we have an enumeration, if yes, return first element
            var valueAsEnumeration = value as IEnumerable;
            if (valueAsEnumeration != null)
            {
                return Extensions.AsSingle(valueAsEnumeration.OfType<object>().FirstOrDefault(), fullResolve);
            }
            else if (value == null)
            {
                return ObjectHelper.Null;
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
        public static IEnumerable<object> AsEnumeration(this object value, bool fullResolve = true)
        {
            // Defines the resolve function
            Func<object, object> resolveFunc;
            if (fullResolve)
            {
                resolveFunc = (x) => Extensions.AsSingle(x, fullResolve);
            }
            else
            {
                resolveFunc = (x) => x;
            }

            // Checks, if value is resolvable
            var valueAsResolvable = value as IResolvable;
            if (valueAsResolvable != null)
            {
                value = resolveFunc(valueAsResolvable.Resolve());
            }

            // Enumerates the elements
            var valueAsEnumeration = value as IEnumerable;
            if (valueAsEnumeration != null)
            {
                foreach (var element in valueAsEnumeration)
                {
                    yield return resolveFunc(element);
                }
            }
            else if (value is IUnspecified)
            {
                var valueAsUnspecified = value as IUnspecified;
                foreach (var element in valueAsUnspecified.AsReflectiveCollection())
                {
                    yield return resolveFunc(element);
                }
            }
            else
            {
                yield return resolveFunc(value);
            }
        }

        public static IEnumerable<T> AsEnumeration<T>(this object value)
        {
            foreach (var item in AsEnumeration(value))
            {
                yield return (T)item;
            }
        }

        public static IReflectiveCollection AsReflectiveCollection(this object value)
        {
            if (value is IReflectiveCollection)
            {
                return value as IReflectiveCollection;
            }

            if (value is IURIExtent)
            {
                return (value as IURIExtent).Elements();
            }

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
            if (value is IReflectiveSequence)
            {
                return value as IReflectiveSequence;
            }

            if (value is IURIExtent)
            {
                return (value as IURIExtent).Elements();
            }

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
            var valueAsSingle = value.AsSingle();
            var asObject = valueAsSingle as IObject;
            if (asObject != null)
            {
                return asObject;
            }

            if (valueAsSingle == null)
            {
                throw new InvalidOperationException("Given Object returned null, when requested as an IObject");
            }
            else
            {
                throw new InvalidOperationException("Given object is of type" + valueAsSingle.GetType().ToString() + ", expected: DatenMeister.IObject");
            }
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
        /// Releases the given extent from pool, so it can be added to a new pool
        /// </summary>
        /// <param name="extent">Extent to be released</param>
        public static void ReleaseFromPool(this IURIExtent extent)
        {
            extent.Pool = null;
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

        public static DateTime? ToDateTime(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is DateTime)
            {
                return (DateTime)value;
            }

            if (value is string)
            {
                DateTime result;
                if (DateTime.TryParse(
                    value.ToString(),
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeLocal,
                    out result))
                {
                    return result;
                }

                // Parsing was not successful
                return null;
            }

            return null;
        }

        /// <summary>
        /// Converts the given object to a string
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Stringified object</returns>
        public static string ToString(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is IFormattable)
            {
                return (value as IFormattable).ToString(null, CultureInfo.InvariantCulture);
            }

            return value.ToString();
        }

        /// <summary>
        /// Creates an object and stores it into the extent
        /// </summary>
        /// <param name="factory">Factory being used to create the object</param>
        /// <param name="extent">Extent, to which the object shall be added</param>
        /// <param name="type">Type of the object which shall be created, may also be null, if the extent supports it</param>
        /// <returns>Object that has been created</returns>
        public static IObject CreateInExtent(this IFactory factory, IURIExtent extent, IObject type = null)
        {
            var result = factory.create(type);
            extent.Elements().add(result);
            return result;
        }

        /// <summary>
        /// Gets the instance within the pool by the extent Uri
        /// </summary>
        /// <param name="pool">Pool to be queried</param>
        /// <param name="extentUri">Uri of the extent</param>
        /// <returns>Found extent id</returns>
        public static ExtentInstance GetInstance(this IPool pool, string extentUri)
        {
            return pool.Instances.Where(x => x.Extent.ContextURI() == extentUri).FirstOrDefault();
        }
    }
}
