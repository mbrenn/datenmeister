using DatenMeister.DataProvider.Generic;
using DatenMeister.Logic;
using DatenMeister.Logic.TypeResolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    /// <summary>
    /// This is the global dotnet-extent, which allows the fast
    /// wrapping of pure .Net objects to DatenMeisterExtents. 
    /// It also assigns the type factories and offers the creation of types via the MOF interface.
    /// 
    /// The extent does not need to be assigned to a certain Pool
    /// </summary>
    public class GlobalDotNetExtent : DotNetExtent, ITypeResolver
    {
        /// <summary>
        /// Defines the uri for the extent. 
        /// </summary>
        public const string GlobalDotNetExtentUri = "datenmeister:///globaldotnextextent";

        /// <summary>
        /// Initializes a new instance of the GlobalDotNetExtent
        /// </summary>
        public GlobalDotNetExtent()
            : base(GlobalDotNetExtentUri)
        {
        }

        /// <summary>
        /// Gets the type by name
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public IObject GetType(string typeName)
        {
            var value =  this.Mapping.FindByName(typeName);
            if ( value != null)
            {
                return value.Type;
            }

            return null;
        }
    }

    /// <summary>
    /// This class contains some additional helper classes for the  GlobalDotNetExtent.
    /// Especially, it is 
    /// </summary>
    public static class GlobalDotNetExtentExtensions
    {
        /// <summary>
        /// Converts the given pure object to a DatenMeister object. 
        /// If necessary, the types and all composite types will be added to the dotnet extent
        /// </summary>
        /// <typeparam name="T">Type of the object to be added</typeparam>
        /// <param name="extent">The extent, which shall store the new type mapping. 
        /// The element will not be added to the given extent</param>
        /// <param name="value">Value to be converted</param>
        /// <returns>The created object</returns>
        public static IObject CreateObject<T>(this GlobalDotNetExtent extent, T value)
        {
            AddTypeMapping(extent, typeof(T));

            return new DotNetObject(extent.Elements(), value);
        }

        /// <summary>
        /// Creates a reflective sequence for the given list of objects
        /// </summary>
        /// <typeparam name="T">Type of the parameter whose list shall be added. </typeparam>
        /// <param name="extent">Extent which shall be used. The return Sequence is then also
        /// capable to add new items. The types of T are registered to the extent</param>
        /// <param name="list">List of objects that shall be converted</param>
        /// <returns>The created sequence that can be used</returns>
        public static IReflectiveSequence CreateReflectiveSequence<T>(this GlobalDotNetExtent extent, IList<T> list)
        {
            AddTypeMapping(extent, typeof(T));

            return new DotNetReflectiveSequence<T>(extent, list);
        }

        /// <summary>
        /// Adds the necessary type mapping for the given type
        /// </summary>
        /// <param name="extent">Extent, where type mapping shall be created</param>
        /// <param name="type">Type, which is used to create the typemapping</param>
        private static DotNetTypeInformation AddTypeMapping(this GlobalDotNetExtent extent, Type type)
        {
            var mapping = extent.Mapping.FindByDotNetType(type);
            if (mapping == null)
            {
                // We need to have a type mapping. First of all, create the type
                var typeObject = new GenericElement(null, type.FullName, DatenMeister.Entities.AsObject.Uml.Types.Type);
                typeObject.set("name", type.ToString());

                mapping = extent.Mapping.Add(type, typeObject);

                // Now go through the properties 
                foreach (var property in type.GetProperties())
                {

                    // Checks, if the given property is an enumeration
                    // If the element is a list or enumeration. 
                    var underlyingListType = ObjectConversion.GetTypeOfEnumerationByType(property.PropertyType);
                    if (underlyingListType != null)
                    {
                        AddTypeMapping(extent, underlyingListType);
                    }
                    else if (!ObjectConversion.IsNativeByType(property.PropertyType))
                    {
                        // Add the type of the property recursively
                        AddTypeMapping(extent, property.PropertyType);
                    }

                    // Add the property to the type
                    var propertyObject = new GenericElement(null, type.FullName, DatenMeister.Entities.AsObject.Uml.Types.Property);
                    propertyObject.set("name", property.Name.ToString());
                    DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(typeObject, propertyObject);
                }
            }
            
            return mapping;
        }

        /// <summary>
        /// Gets the type object for a specific type
        /// </summary>
        /// <param name="extent">Type of the extent</param>
        /// <param name="type">DotNetType to be queried</param>
        /// <returns>Found object</returns>
        public static IObject GetIObjectForType(this GlobalDotNetExtent extent, Type type)
        {
            return extent.AddTypeMapping(type).Type;
        }
    }
}
