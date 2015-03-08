using BurnSystems.Test;
using DatenMeister.DataProvider;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic.TypeConverter
{
    /// <summary>
    /// Converts the dot net type
    /// </summary>
    public class DotNetTypeConverter : IDotNetTypeConverter
    {
        /// <summary>
        /// Gets or sets the factory provider, will be injected
        /// </summary>
        [Inject]
        public IFactoryProvider FactoryProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Converts the given dotnet type to an IObject type and adds it to the given extent 
        /// </summary>
        /// <param name="type">Type to be converted</param>
        /// <returns>Converted object</returns>
        public IObject Convert(IURIExtent extent, Type type)
        {
            Ensure.That(this.FactoryProvider != null, "No Factory Provider is given");
            Ensure.That(DatenMeister.Entities.AsObject.Uml.Types.Type != null, "UML objects are not initialized (Class)");
            Ensure.That(DatenMeister.Entities.AsObject.Uml.Types.Property != null, "UML objects are not initialized (Property)");

            var factory = this.FactoryProvider.CreateFor(extent);
            var typeObject = Convert(factory, type);

            extent.Elements().add(typeObject);

            return typeObject;
        }

        /// <summary>
        /// Converts the given dotnet type to an IObject type and uses the given Factory for conversion
        /// </summary>
        /// <param name="factory">Factory to be used to create the item</param>
        /// <param name="type">Type to be converted</param>
        /// <returns>Converted object</returns>
        public IObject Convert(IFactory factory, Type type, Action<Type> callBackInnerTypes = null)
        {
            // We need to have a type mapping. First of all, create the type
            var typeObject = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
            typeObject.set("name", type.ToString());

            // Now go through the properties 
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // Checks, if the given property is an enumeration
                // If the element is a list or enumeration. 
                var underlyingListType = ObjectConversion.GetTypeOfEnumerableByType(property.PropertyType);
                if (underlyingListType != null)
                {
                    if (callBackInnerTypes != null)
                    {
                        callBackInnerTypes(underlyingListType);
                    }
                    else
                    {
                        throw new InvalidOperationException("No inner properties supported.");
                    }
                }
                else if (!ObjectConversion.IsNativeByType(property.PropertyType))
                {
                    if (callBackInnerTypes != null)
                    {
                        callBackInnerTypes(property.PropertyType);
                    }
                    else
                    {
                        throw new InvalidOperationException("No inner properties supported.");
                    }
                }

                // Add the property to the type
                var propertyObject = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                propertyObject.set("name", property.Name.ToString());
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(typeObject, propertyObject);
            }

            return typeObject;
        }
    }
}
