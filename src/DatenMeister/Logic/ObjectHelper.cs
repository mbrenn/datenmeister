﻿using System;
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