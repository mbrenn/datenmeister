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
        /// Gets the property as a single property. 
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <param name="propertyName">Name of the property to be retrieved</param>
        /// <returns>The retrieved object as returned by the Object</returns>
        public static object getAsSingle(
            this IObject value, 
            string propertyName)
        {
            return value.get(propertyName, RequestType.AsSingle).FullResolve();
        }

        /// <summary>
        /// Gets the property as a reflective sequence
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <param name="propertyName">Name of the property to be retrieved</param>
        /// <returns>The retrieved object as returned by the Object. 
        /// If the given object is not a reflective sequence, an exception is thrown</returns>
        public static IReflectiveSequence getAsReflectiveSequence(
            this IObject value,
            string propertyName)
        {
            if (value == null)
            {
                // We are null, so we return null
                return null;
            }

            var result = value.get(propertyName, RequestType.AsReflectiveCollection).FullResolve();
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
        /// Fully resolves the given object. 
        /// It will try to convert the object into IResolvable and then return 
        /// the resolved value
        /// </summary>
        /// <param name="value">Value to be resolved</param>
        /// <returns>The resolved object</returns>
        public static object FullResolve(this object value)
        {
            var valueAsResolvable = value as IResolvable;
            if (valueAsResolvable != null)
            {
                var result = valueAsResolvable.Resolve();
                return FullResolve(result);
            }

            return value;
        }

        /// <summary>
        /// Gets the given object as an IObject interface.
        /// It also resolves the given object by calling full resolve
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>Returns given object, if it is an IObject</returns>
        public static IObject AsIObjectOrNull(this object value)
        {
            if ( value == null)
            {
                return null;
            }
            else
            {
                return AsIObject(value);
            }
        }

        /// <summary>
        /// Gets the given object as an IObject interface.
        /// It also resolves the given object by calling full resolve
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>Returns given object, if it is an IObject</returns>
        public static IObject AsIObject(this object value)
        {
            value = FullResolve(value);

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

        /// <summary>
        /// Releases the given extent from pool, so it can be added to a new pool
        /// </summary>
        /// <param name="extent">Extent to be released</param>
        public static void ReleaseFromPool(this IURIExtent extent)
        {
            extent.Pool = null;
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
