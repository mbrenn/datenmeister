using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.SourceFactory
{
    public interface ITypeInfoProvider
    {
        /// <summary>
        /// Returns the types
        /// </summary>
        /// <returns>Types of the provider</returns>
        IEnumerable<string> GetTypes();

        /// <summary>
        /// Gets the properties of a given type
        /// </summary>
        /// <param name="type">Type, whose properties are requested</param>
        /// <returns>Enumeration of properties</returns>
        IEnumerable<string> GetProperties(string type);

    }
}
