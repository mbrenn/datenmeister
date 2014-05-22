using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    /// <summary>
    /// Defines the interface for factory in alignment to MOF CoreSpecification 2.4.1, Chapter 9.2
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Creates an additional object.
        /// </summary>
        /// <param name="type">Type of the object. May be null for 'any' type</param>
        /// <returns>Object that shall be created</returns>
        IObject create(IObject type);

        /// <summary>
        /// Creates am object by an object and its given datatype
        /// </summary>
        /// <param name="dataType">Datatype to be used</param>
        /// <param name="value">Value to be used</param>
        /// <returns>Created object</returns>
        IObject createFromString(IObject dataType, string value);

        /// <summary>
        /// Converts the object to a string
        /// </summary>
        /// <param name="dataType">Datatype of the object</param>
        /// <param name="value">Object to be converted to a string</param>
        /// <returns>String representation of the object</returns>
        string convertToString(IObject dataType, IObject value);
    }
}
