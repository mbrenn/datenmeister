using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.SourceFactory
{
    /// <summary>
    /// Defines the interface which is necessary to create TypeScript files
    /// </summary>
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
        /// <param name="typeName">Type, whose properties are requested</param>
        /// <returns>Enumeration of properties</returns>
        IEnumerable<string> GetProperties(string typeName);

        /// <summary>
        /// Returns the type of a property
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <param name="propertyName">Type, whose property will be retrieved</param>
        /// <returns>Type of the given property</returns>
        Type GetTypeOfProperty(string typeName, string propertyName);

        /// <summary>
        /// Gets the default value for a certain property. 
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Default value or null, if property has no default value</returns>
        object GetDefaultValueForProperty(string typeName, string propertyName);

        /// <summary>
        /// Returns the argument-names for the constructor 
        /// If there is no constructor, an empty list will be returned, 
        /// if there are more than one constructors, an exception will be thrown.
        /// Only the arguments of the class itself, not the ones from base classes will be returned
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <returns>List of names of the arguments.</returns>
        List<string> GetArgumentsForConstructor(string typeName);

        /// <summary>
        /// Gets the full type name for the given type, which had been retrieved by <c>GetTypes</c>
        /// </summary>
        /// <returns>Full name of type </returns>
        string GetFullTypeName(string type);
    }
}
