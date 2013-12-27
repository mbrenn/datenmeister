//-----------------------------------------------------------------------
// <copyright file="TypedContainer.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implements a container, that stores the instances according to their type. 
    /// An instance can be retrieved by its type
    /// </summary>
    public class TypedContainer
    {
        /// <summary>
        /// Stores the object that has been stored within the instances
        /// </summary>
        private List<object> instances = new List<object>();

        /// <summary>
        /// Adds an object to the container
        /// </summary>
        /// <param name="instance">Instance to be added</param>
        public void Add(object instance)
        {
            this.instances.Add(instance);
        }

        /// <summary>
        /// Gets the first object matching to this type. 
        /// If multiple objects of the type are available, an InvalidOperationException will be thrown.
        /// </summary>
        /// <typeparam name="T">Type to be requested</typeparam>
        /// <returns>Object or null, if not found</returns>
        public T Get<T>()
        {
            T found = default(T);
            var alreadyFound = false;

            foreach (var instance in this.instances)
            {
                if (instance is T)
                {
                    if (alreadyFound)
                    {
                        throw new InvalidOperationException(LocalizationBS.TypedContainer_MultipleInstance);
                    }

                    found = (T) instance;
                    alreadyFound = true;
                }
            }

            return found;
        }

        /// <summary>
        /// Gets all objects matching to a certain type
        /// </summary>
        /// <typeparam name="T">Type to be requested</typeparam>
        /// <returns>Enumeration of found types</returns>
        public IEnumerable<T> GetAll<T>()
        {
            return this.instances.
                Where(x => x is T)
                .Cast<T>();
        }
    }
}
