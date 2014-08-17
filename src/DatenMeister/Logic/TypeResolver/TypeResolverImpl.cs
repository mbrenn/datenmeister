using BurnSystems.ObjectActivation;
using DatenMeister.Entities.AsObject.Uml;
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
        public IObject GetType(string typeName)
        {
            // Gets the property
            var pool = Global.Application.Get<IPool>();
            var type = pool.Instances.SelectMany(x => x.Extent.Elements()
                .Where(y => y is IObject)
                .Cast<IObject>()
                .Where(y => NamedElement.getName(y) == typeName)).FirstOrDefault();

            if (type != null)
            {
                return type;
            }

            return null;
        }
    }
}
