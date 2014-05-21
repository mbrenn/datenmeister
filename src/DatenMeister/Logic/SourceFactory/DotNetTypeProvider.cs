 using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DatenMeister.Logic.SourceFactory
{
    public class DotNetTypeProvider : ITypeInfoProvider
    {
        /// <summary>
        /// Stores the list of types
        /// </summary>
        private List<Type> types;

        public DotNetTypeProvider(IEnumerable<Type> types)
        {
            Ensure.That(types != null);
            this.types = new List<Type>();
            this.types.AddRange(types);
        }

        /// <summary>
        /// Gets the types of the info provider
        /// </summary>
        /// <returns>Enumeration of types</returns>
        public IEnumerable<string> GetTypes()
        {
            return this.types.Select(x => x.Name);
        }

        /// <summary>
        /// Gets the properties of a certain type
        /// </summary>
        /// <param name="typeName">Name of the type, which will be queried</param>
        /// <returns>Enumeration of properties to the type</returns>
        public IEnumerable<string> GetProperties(string typeName)
        {
            var type = this.FindType(typeName);

            return type.GetProperties().Where(x => x.CanRead && x.CanWrite).Select(x => x.Name);
        }

        /// <summary>
        /// Returns the type of a property
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <param name="propertyName">Type, whose property will be retrieved</param>
        /// <returns>Type of the given property</returns>
        public Type GetTypeOfProperty(string typeName, string propertyName)
        {
            var type = this.FindType(typeName);

            var resultProperty = FindProperty(type, propertyName);

            if (resultProperty == null)
            {
                return null;
            }

            return resultProperty.PropertyType;
        }

        /// <summary>
        /// Returns the argument-names for the constructor 
        /// If there is no constructor, an empty list will be returned, 
        /// if there are more than one constructors, an exception will be thrown.
        /// Only the arguments of the class itself, not the ones from base classes will be returned
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <returns>List of names of the arguments.</returns>
        public List<string> GetArgumentsForConstructor(string typeName)
        {
            var type = this.FindType(typeName);

            var constructors = type.GetConstructors(
                System.Reflection.BindingFlags.DeclaredOnly | 
                System.Reflection.BindingFlags.Instance | 
                System.Reflection.BindingFlags.Public);

            if (constructors.Length == 0)
            {
                return new List<string>();
            }

            if ( constructors.Length > 1 )
            {
                constructors = constructors.Where(x => x.GetParameters().Count() > 0).ToArray();
            }

            Ensure.That(constructors.Length <= 1, typeName + " has more than one constructor");
            var constructor = constructors.First();

            return constructor.GetParameters().Select(x => x.Name).ToList();
        }

        /// <summary>
        /// Gets the default value for a certain property. 
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Default value or null, if property has no default value</returns>
        public object GetDefaultValueForProperty(string typeName, string propertyName)
        {
            var type = this.FindType(typeName);
            var property = FindProperty(type, propertyName);

            var defaultValueAttribute =
                property
                    .GetCustomAttributes(typeof(DefaultValueAttribute), false).FirstOrDefault()
                as DefaultValueAttribute;

            if (defaultValueAttribute == null)
            {
                return null;
            }

            // Gets value
            return defaultValueAttribute.Value;
        }
        
        /// <summary>
        /// Gets the full type name for the given type, which had been retrieved by <c>GetTypes</c>
        /// </summary>
        /// <returns>Full name of type </returns>
        public string GetFullTypeName(string typeName)
        {
            var type = this.FindType(typeName);
            return type.FullName;
        }

        /// <summary>
        /// Finds the type by name
        /// </summary>
        /// <param name="typeName">Name of type</param>
        /// <returns>Type being found</returns>
        private Type FindType(string typeName)
        {
            Ensure.That(typeName != null);

            var type = this.types.First(x => x.Name == typeName);
            Ensure.That(type != null);
            return type;
        }

        /// <summary>
        /// Finds the property
        /// </summary>
        /// <param name="type">Name of type</param>
        /// <param name="propertyName">Name of property</param>
        /// <returns>PropertyInfo of the property or null</returns>
        private static PropertyInfo FindProperty(Type type, string propertyName)
        {
            var resultProperty = type.GetProperties().Where(x => x.CanRead && x.CanWrite).Where(x => x.Name == propertyName).FirstOrDefault();
            return resultProperty;
        }
    }
}
