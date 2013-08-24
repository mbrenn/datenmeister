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
    }
}
