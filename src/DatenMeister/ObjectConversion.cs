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

            return value.ToString();
        }
    }
}
