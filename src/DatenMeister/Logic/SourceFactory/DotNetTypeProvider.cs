using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Ensure.That(typeName != null);

            var type = this.types.First(x => x.Name == typeName);
            Ensure.That(type != null);

            return type.GetProperties().Where(x => x.CanRead && x.CanWrite).Select(x => x.Name);
        }

        /// <summary>
        /// Returns the type of a property
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <param name="property">Type, whose property will be retrieved</param>
        /// <returns>Type of the given property</returns>
        public Type GetTypeOfProperty(string typeName, string property)
        {
            var type = this.FindType(typeName);

            var resultProperty = type.GetProperties().Where(x => x.CanRead && x.CanWrite).Where(x => x.Name == property).FirstOrDefault();
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

            Ensure.That(constructors.Length <= 1, typeName + " has more than one constructor");
            var constructor = constructors.First();

            return constructor.GetParameters().Select(x => x.Name).ToList();
        }

        private Type FindType(string typeName)
        {
            Ensure.That(typeName != null);

            var type = this.types.First(x => x.Name == typeName);
            Ensure.That(type != null);
            return type;
        }
    }
}
