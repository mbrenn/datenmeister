using BurnSystems.ObjectActivation;
using DatenMeister.Entities.AsObject.Uml;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.TypeResolver
{
    /// <summary>
    /// Tries to find a certain type by name.
    /// Is quite a simple thing, which goes through every object in all extents, 
    /// filters the types and returns the one with the correct object.
    /// 
    /// In addition, the types will be cached to improve speed
    /// </summary>
    public class TypeResolverImpl : ITypeResolver
    {
        /// <summary>
        /// Defines the cache for the type resolver. It just works, when 
        /// the object is singleton or something like that
        /// </summary>
        private Dictionary<string, IObject> cache = new Dictionary<string, IObject>();

        /// <summary>
        /// Returned type by name
        /// </summary>
        /// <param name="typeName">Name of the type</param>
        /// <returns>Found type</returns>
        public IObject GetType(string typeName)
        {
            IObject result;
            if (this.cache.TryGetValue(typeName, out result))
            {
                return result;
            }

            // Gets the property
            var pool = Injection.Application.Get<IPool>();
            var type = pool.GetExtents().SelectMany(x => x.Elements()
                .Where(y => y is IObject)
                .Cast<IObject>()
                .Where(y => NamedElement.getName(y) == typeName)).FirstOrDefault();

            if (type != null)
            {
                this.cache[typeName] = type;
                return type;
            }

            return null;
        }
    }
}
