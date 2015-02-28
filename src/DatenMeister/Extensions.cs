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
        /// Gets the given object as an IObject interface
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>Returns given object, if it is an IObject</returns>
        public static IObject AsIObject(this object value)
        {
            var asObject = value as IObject;
            if (asObject != null)
            {
                return asObject;
            }

            if (value == ObjectHelper.NotSet)
            {
                return null;
            }

            if (value == null)
            {
                throw new InvalidOperationException("Given Object returned null, when requested as an IObject");
            }
            else
            {
                throw new InvalidOperationException("Given object is of type" + value.GetType().ToString() + ", expected: DatenMeister.IObject");
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
