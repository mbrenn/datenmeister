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

            if (targetType.IsAssignableFrom(value.GetType()))
            {
                return value;
            }

            return Convert.ChangeType(value, targetType);
        }
    }
}
