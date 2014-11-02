using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    /// <summary>
    /// Stores a lot of helper methods which are used for object conversion
    /// </summary>
    public static class ObjectConversion
    {
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

            var valueAsUnspecified = value as IUnspecified;
            if (valueAsUnspecified != null)
            {
                return ToBoolean(valueAsUnspecified.AsSingle());
            }

            return false;
        }

        public static int ToInt32(object value)
        {
            if (value == null)
            {
                return 0;
            }

            if (value is Int32 || value is Int16)
            {
                return (Int32)value;
            }

            if (value is string)
            {
                Int32 result;
                if (Int32.TryParse(value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                {
                    return result;
                }
            }

            var valueAsUnspecified = value as IUnspecified;
            if (valueAsUnspecified != null)
            {
                return ToInt32(valueAsUnspecified.AsSingle());
            }

            return 0;
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

            var valueAsUnspecified = value as IUnspecified;
            if (valueAsUnspecified != null)
            {
                return ToDateTime(valueAsUnspecified.AsSingle());
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
            if (value == null || value == ObjectHelper.Null || value == ObjectHelper.NotSet)
            {
                return null;
            }

            if (value is IFormattable)
            {
                return (value as IFormattable).ToString(null, CultureInfo.InvariantCulture);
            }

            var valueAsUnspecified = value as IUnspecified;
            if (valueAsUnspecified != null)
            {
                return ToString(valueAsUnspecified.AsSingle());
            }

            return value.ToString();
        }

        /// <summary>
        /// Converts the object to the target type
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="targetType">Type, to which the target shall be converted</param>
        /// <returns>The converted type</returns>
        public static object ConvertTo(object value, Type targetType)
        {

            var valueAsUnspecified = value as IUnspecified;
            if (valueAsUnspecified != null)
            {
                return ConvertTo(valueAsUnspecified.AsSingle(), targetType);
            }

            if (targetType == typeof(Int32))
            {
                return ToInt32(value);
            }

            if (targetType == typeof(String))
            {
                return ToString(value);
            }

            if (targetType == typeof(Boolean))
            {
                return ToBoolean(value);
            }

            if (targetType == typeof(DateTime))
            {
                return ToDateTime(targetType);
            }

            if(targetType.IsAssignableFrom(value.GetType()))
            {
                return value;
            }

            return Convert.ChangeType(value, targetType);
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

            var type = checkObject.GetType();
            return IsNativeByType(type);
        }

        /// <summary>
        /// Checks, if a certain type is a native .Net object
        /// </summary>
        /// <param name="type">Type to be considered</param>
        /// <returns>true, if the object type is native</returns>
        public static bool IsNativeByType(Type type)
        {
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
            return primitiveTypes.Contains(type);
        }

        /// <summary>
        /// Checks, if the object is an enumeration object
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>true, if the object is an enumeratio</returns>
        public static bool IsEnumeration(object value)
        {
            return IsEnumerationByType(value.GetType());
        }

        /// <summary>
        /// Checks, if the given type is an enumeration type. It just calls IsEnum
        /// </summary>
        /// <param name="type">Type to be checked</param>
        /// <returns></returns>
        public static bool IsEnumerationByType(Type type)
        {
            return type.IsEnum;
        }

        /// <summary>
        /// Converts an object to an enumeration.
        /// It can be a string or any other object
        /// </summary>
        /// <param name="value">Value to be convered</param>
        /// <param name="type">Type to be used for this conversion</param>
        /// <returns>The enumeration object of type 'type'</returns>
        public static object ConvertToEnumeration(object value, Type type)
        {
            if (value.GetType() == type)
            {
                return value;
            }

            var valueAsString = value.ToString();
            // TryParse
            try
            {
                return Enum.Parse(type, valueAsString);
            }
            catch
            {
                // Per default, return the first name, if not found
                return Enum.GetNames(type).First();
            }
        }
    }
}
