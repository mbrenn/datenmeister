using DatenMeister.DataProvider;
using DatenMeister.Entities.DM;
using DatenMeister.Logic;
using DatenMeister.Pool;
using Ninject;
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
        /// Gets the extent instance of a certain extent. 
        /// The pool will be retrieved from the DoI container
        /// </summary>
        /// <param name="extent">Extent whose instance is queried</param>
        /// <returns>Found extent instance</returns>
        public static ExtentInfo GetInstance(this IURIExtent extent)
        {
            var pool = Injection.Application.Get<IPool>();
            return pool.GetInstance(extent);
        }

        /// <summary>
        /// Gets the property as a single property. 
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <param name="propertyName">Name of the property to be retrieved</param>
        /// <returns>The retrieved object as returned by the Object</returns>
        public static object getAsSingle(this IObject value, string propertyName)
        {
            return value.get(propertyName, RequestType.AsSingle);
        }

        /// <summary>
        /// Gets the property as a reflective sequence
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <param name="propertyName">Name of the property to be retrieved</param>
        /// <returns>The retrieved object as returned by the Object. 
        /// If the given object is not a reflective sequence, an exception is thrown</returns>
        public static IReflectiveSequence getAsReflectiveSequence(this IObject value, string propertyName)
        {
            var result = value.get(propertyName, RequestType.AsReflectiveCollection);
            if (result == null)
            {
                return null;
            }

            if (result is IReflectiveSequence)
            {
                return result as IReflectiveSequence;
            }

            throw new InvalidOperationException(
                string.Format("{0} did not return IReflectiveSequence on property '{1}'. Was: {2}",
                value.ToString(),
                propertyName,
                value.GetType().FullName));
        }

        /// <summary>
        /// If the given object is an enumeration, it returns the first instance of the given object.
        /// Otherwise it returns the object itself. 
        /// If the object is null or enumeration is empty, null will be returned
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Converted object</returns>
        [Obsolete]
        public static object AsSingle(this object value, bool fullResolve = true)
        {
            if (value == null)
            {
                return ObjectHelper.Null;
            }

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

            // Checks, if we have an enumeration, if yes, return first element
            var valueAsEnumeration = value as IEnumerable;
            if (valueAsEnumeration != null)
            {
                return Extensions.AsSingle(valueAsEnumeration.OfType<object>().FirstOrDefault(), fullResolve);
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
        [Obsolete]
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
                // Single object, if single object is NotSet, skip it
                if (value != ObjectHelper.NotSet)
                {
                    yield return resolveFunc(value);
                }
            }
        }

        [Obsolete]
        public static IEnumerable<T> AsEnumeration<T>(this object value)
        {
            foreach (var item in AsEnumeration(value))
            {
                yield return (T)item;
            }
        }

        [Obsolete]
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

        [Obsolete]
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

            if (valueAsSingle == ObjectHelper.NotSet)
            {
                return null;
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
            if (ObjectConversion.IsNative(pairValue))
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

        public static void Set(this IObject value, Dictionary<string, object> values)
        {
            foreach (var pair in values)
            {
                value.set(pair.Key, pair.Value);
            }
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
    }
}
