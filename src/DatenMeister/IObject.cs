using BurnSystems.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    /// <summary>
    /// Defines the interface for every object that has properties
    /// </summary>
    public interface IObject
    {
        /// <summary>
        /// Gets the property by propertyname
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        object Get(string propertyName);

        /// <summary>
        /// Gets all properties as key value pairs
        /// </summary>
        /// <returns></returns>
        IEnumerable<Pair<string, object>> GetAll();

        bool IsSet(string propertyName);

        void Set(string propertyName, object value);

        void Unset(string propertyName);
    }
}
