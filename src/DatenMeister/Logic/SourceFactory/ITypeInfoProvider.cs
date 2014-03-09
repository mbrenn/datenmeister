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

        /// <summary>
        /// Returns the type of a property
        /// </summary>
        /// <param name="type">Name of the type</param>
        /// <param name="property">Type, whose property will be retrieved</param>
        /// <returns>Type of the given property</returns>
        Type GetTypeOfProperty(string type, string property);

        /// <summary>
        /// Returns the argument-names for the constructor 
        /// If there is no constructor, an empty list will be returned, 
        /// if there are more than one constructors, an exception will be thrown.
        /// Only the arguments of the class itself, not the ones from base classes will be returned
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <returns>List of names of the arguments.</returns>
        List<string> GetArgumentsForConstructor(string typeName);

    }
}
