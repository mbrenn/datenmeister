using DatenMeister.DataProvider;
using DatenMeister.Logic.SourceFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.Views
{
    /// <summary>
    /// Contains some helper methods to create view definitions for 
    /// </summary>
    public static class ViewHelper
    {
        /// <summary>
        /// Performs the autogeneration of the view definition, in dependence of the
        /// only type in <c>provider</c>.
        /// </summary>
        /// <param name="viewDefinition">View Definition that shall be filled out</param>
        /// <param name="typeInfo">Type, which shall be included</param>
        public static void AutoGenerateViewDefinition(IObject viewDefinition, Type typeInfo)
        {
            AutoGenerateViewDefinition(viewDefinition, new DotNetTypeProvider(new[] { typeInfo }));
        }

        /// <summary>
        /// Performs the autogeneration of the view definition, in dependence of the
        /// only type in <c>provider</c>.
        /// </summary>
        /// <param name="viewDefinition">View Definition that shall be filled out</param>
        /// <param name="provider">Provider to be used</param>
        public static void AutoGenerateViewDefinition(IObject viewDefinition, ITypeInfoProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Auto generates the view definitions for a complete extent. 
        /// The information will be stored as a reflective collection in viewInfo. 
        /// </summary>
        /// <param name="extent">Extent, being used</param>
        /// <param name="viewInfo">View Information, where the objects will be stored</param>
        public static void AutoGenerateViewDefinition(IURIExtent extent, IObject viewInfo, bool orderByName = false)
        {
            AutoGenerateViewDefinition(extent.Elements(), viewInfo, orderByName);
        }

        /// <summary>
        /// Auto generates the view definitions for a complete extent. 
        /// The information will be stored as a reflective collection in viewInfo. 
        /// </summary>
        /// <param name="extent">Extent, being used</param>
        /// <param name="viewInfo">View Information, where the objects will be stored</param>
        public static void AutoGenerateViewDefinition(IReflectiveCollection collection, IObject viewInfo, bool orderByName = false)
        {
            var factory = Factory.GetFor(viewInfo.Extent);
            var fieldInfos = viewInfo.get("fieldInfos").AsReflectiveSequence();

            var propertyNames = collection.GetConsolidatedPropertyNames();
            if (orderByName == true)
            {
                propertyNames = propertyNames.OrderBy(x => x.ToLower() == "id" ? string.Empty : x);
            }

            foreach (var name in propertyNames)
            {
                var textField = factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.TextField);
                var textFieldObj = new DatenMeister.Entities.AsObject.FieldInfo.TextField(textField);
                textFieldObj.setName(name);
                textFieldObj.setBinding(name);

                if (name == "id")
                {
                    textFieldObj.setReadOnly(true);
                }

                fieldInfos.add(textField);
            }
        }
    }
}
