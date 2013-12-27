using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BurnSystems.Interfaces
{
    /// <summary>
    /// This interface defines a converter which converts an entity to an XElement-Node and vice versa
    /// </summary>
    /// <typeparam name="T">Type of the entity to be converted</typeparam>
    public interface IXElementConverter<T>
    {
        /// <summary>
        /// Converts the xml element to an entity. 
        /// If the item can not be converted, null shall be returned
        /// </summary>
        /// <param name="element">Element to be created</param>
        /// <returns>Returns the converted element</returns>
        T Convert(XElement element);

        /// <summary>
        /// Converts the entity to the xml node
        /// </summary>
        /// <param name="entity">Entity to be converted</param>
        /// <returns>Converted element</returns>
        XElement Convert(T entity);
    }
}
