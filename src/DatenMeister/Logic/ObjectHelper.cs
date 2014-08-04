using System;
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
                var b2 = DatenMeister.Extensions.ToBoolean(v2);
                return b1 == b2;
            }

            if (v1 is DateTime)
            {
                var d1 = (DateTime)v1;
                var d2 = DatenMeister.Extensions.ToDateTime(v2);
                return d1 == d2;
            }

            if (v1 is string)
            {
                var s1 = (string)v1;
                var s2 = DatenMeister.Extensions.ToString(v2);
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
        /// Just used for the Null object
        /// </summary>
        private class NullObject
        {
            public override string ToString()
            {
                return "NULL";
            }
        }
    }
}