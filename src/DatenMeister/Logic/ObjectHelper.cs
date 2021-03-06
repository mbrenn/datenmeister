﻿using DatenMeister.DataProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    /// <summary>
    /// Stores several helper methods to handle IObjects
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// Gets an enumeration of column titles for a set of objects
        /// </summary>
        /// <param name="objects">Objects to be examined</param>
        /// <returns></returns>
        public static IEnumerable<string> GetColumnNames(this IEnumerable<IObject> objects)
        {
            var titles = objects.SelectMany(x => x.getAll().Select(y => y.PropertyName)).Distinct();
            return titles;
        }

        /// <summary>
        /// This value has to be returned, if a property has been requested from an IObject an is not available
        /// </summary>
        public static object NotSet = new NotSetObject();

        public static object Null = new NullObject();

        /// <summary>
        /// This value is used, when the common value of multiple objects is requested
        /// and there is no common value. This indicates, that not all properties
        /// have the same value
        /// </summary>
        public static object Different = new DifferentObject();

        /// <summary>
        /// Checks, if the given objects are equal
        /// </summary>
        /// <param name="v1">Object to be checked as primary, which defines the target type</param>
        /// <param name="v2">Object to be checked as secondary</param>
        /// <returns>true, if the given objects are equal</returns>
        public static bool AreEqual(object v1, object v2)
        {
            if (v2 == null || v2 == ObjectHelper.NotSet || v2 == ObjectHelper.Null)
            {
                return v1 == null || v1 == ObjectHelper.NotSet || v1 == ObjectHelper.Null;
            }

            if (v1 is bool)
            {
                var b1 = (bool)v1;
                var b2 = DatenMeister.ObjectConversion.ToBoolean(v2);
                return b1 == b2;
            }

            if (v1 is DateTime)
            {
                var d1 = (DateTime)v1;
                var d2 = DatenMeister.ObjectConversion.ToDateTime(v2);
                return d1 == d2;
            }

            if (v1 is string)
            {
                var s1 = (string)v1;
                var s2 = DatenMeister.ObjectConversion.ToString(v2);
                return s1 == s2;
            }

            return v1.Equals(v2);
        }

        /// <summary>
        /// Just used for the NotSet object
        /// </summary>
        private class NotSetObject
        {
            public override string ToString()
            {
                return "Not Set";
            }
        }

        /// <summary>
        /// Just used for the NotSet object
        /// </summary>
        private class DifferentObject
        {
            public override string ToString()
            {
                return "Different";
            }
        }

        /// <summary>
        /// Just used for the Null object
        /// </summary>
        private class NullObject
        {
            public override string ToString()
            {
                return "NULL";
            }
        }

        /// <summary>
        /// Checks whether the given object is null, ObjectHelper.Null or ObjectHelper.NotSet
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>true, if the object returns null</returns>
        public static bool IsNull(object value)
        {
            value = value.AsSingle();
            return value == null
                || value == ObjectHelper.Null
                || value == ObjectHelper.NotSet;
        }

        /// <summary>
        /// Checks whether the given object is a reflective collection
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>true, if the object is a reflective </returns>
        public static bool IsReflectiveCollection(object value)
        {
            if (value is IReflectiveCollection)
            {
                return true;
            }

            var valueAsUnspecified = value as IUnspecified;
            if (valueAsUnspecified != null &&
                valueAsUnspecified.PropertyValueType == PropertyValueType.Enumeration)
            {
                return true;
            }

            var valueAsProxyObject = value as IProxyObject;
            if (valueAsProxyObject != null)
            {
                return IsReflectiveCollection(valueAsProxyObject.Value);
            }

            return false;
        }

        /// <summary>
        /// Gets the common value of all properties 
        /// </summary>
        /// <param name="objects">Objects to be checked</param>
        /// <param name="propertyName">Property, that shall be used</param>
        /// <returns>
        /// The common value or ObjectHelper.NotSet all properties are not set
        /// or ObjectHelper.Different if at least one element is different to the other
        /// </returns>
        public static object GetCommonValue(IEnumerable<IObject> objects, string propertyName)
        {
            // check if everything is set or not set. And if it set, that it has the same content
            var nothingIsSet = true;
            var everythingIsSet = true;
            var noElement = true;
            var firstRoundIndicator = new object();

            if (objects == null)
            {
                return null;
            }

            // Checks
            object value = firstRoundIndicator;
            foreach (var detailObject in objects)
            {
                noElement = false;

                var isSet = detailObject.isSet(propertyName);
                if (isSet)
                {
                    nothingIsSet = false;

                    if (value != ObjectHelper.Different)
                    {
                        var propertyValue = detailObject.get(propertyName);

                        if (value == firstRoundIndicator)
                        {
                            value = propertyValue;
                        }                        
                        else if (!propertyValue.Equals(value))
                        {
                            value = ObjectHelper.Different;
                        }
                    }
                }
                else
                {
                    value = ObjectHelper.Different;
                    everythingIsSet = false;
                }
            }

            // No element is given
            if (noElement)
            {
                return ObjectHelper.Null;
            }

            // Checks, if it is reasonable
            if (everythingIsSet && nothingIsSet)
            {
                throw new InvalidOperationException("Not Possible");
            }

            if (nothingIsSet)
            {
                return ObjectHelper.NotSet;
            }
            else
            {
                return value;
            }
        }
    }
}