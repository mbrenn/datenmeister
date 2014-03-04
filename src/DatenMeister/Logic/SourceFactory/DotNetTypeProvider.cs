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
    }
}
