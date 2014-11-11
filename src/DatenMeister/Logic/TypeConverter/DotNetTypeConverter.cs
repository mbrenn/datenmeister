using BurnSystems.Test;
using DatenMeister.DataProvider;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
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

            // We need to have a type mapping. First of all, create the type
            var typeObject = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
            typeObject.set("name", type.ToString());

            // Now go through the properties 
            foreach (var property in type.GetProperties())
            {
                if (!Extensions.IsNativeByType(property.PropertyType))
                {
                    // Add the type of the property recursively
                    throw new NotImplementedException("Subtypes are not exported");
                }

                // Add the property to the type
                var propertyObject = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
                propertyObject.set("name", property.Name.ToString());
                DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute(typeObject, propertyObject);
            }

            extent.Elements().add(typeObject);

            return typeObject;
        }
    }
}
