using DatenMeister.DataProvider;
using DatenMeister.DataProvider.DotNet;
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
        /// This container stores the view types as an assignment table. 
        /// It can be used for DotNetSequences or other implicit conversions
        /// </summary>
        public static DotNetExtent ViewTypes
        {
            get;
            private set;
        }

        static ViewHelper()
        {
            ViewTypes = new DotNetExtent("not used");
            DatenMeister.Entities.AsObject.FieldInfo.Types.AssignTypeMapping(ViewTypes);
        }

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
        /// Auto generates the view definitions for a single obect 
        /// The information will be stored as a reflective collection in viewInfo. 
        /// </summary>
        /// <param name="extent">Extent, being used</param>
        /// <param name="viewInfo">View Information, where the objects will be stored</param>
        public static void AutoGenerateViewDefinition(IObject value, IObject viewInfo, bool orderByName = false)
        {
            AutoGenerateViewDefinition(new object[] { value }, viewInfo, orderByName);
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
        public static void AutoGenerateViewDefinition(IEnumerable<object> collection, IObject viewInfo, bool orderByName = false)
        {
            var factory = Factory.GetFor(viewInfo);
            var fieldInfos = viewInfo.get("fieldInfos").AsReflectiveSequence();
            var info = collection.GetConsolidatedInformation();

            // Gets the type, if necessary
            if (info.TypeCount > 1)
            {
                fieldInfos.add(AddTextField(factory, "Type", ObjectDictionaryForView.TypeBinding));
            }

            // Gets the propertynames
            var propertyNames = info.PropertyNames;
            if (orderByName == true)
            {
                propertyNames = propertyNames.OrderBy(x => x.ToLower() == "id" ? string.Empty : x);
            }

            // Performs the creation of fields by propertyname
            foreach (var name in propertyNames)
            {
                fieldInfos.add(AddTextField(factory, name));
            }

            // Gets the extentUri, if necessary
            if (info.ExtentCount > 1)
            {
                fieldInfos.add(AddTextField(factory, "ExtentUri", ObjectDictionaryForView.ExtentUriBinding));
            }
        }

        /// <summary>
        /// Adds the text field for a certain name and binding
        /// </summary>
        /// <param name="factory">Factory to be used to create the textfield</param>
        /// <param name="name">Name of the property</param>
        /// <param name="binding">Name for the binding</param>
        /// <returns>Created object</returns>
        private static IObject AddTextField(IFactory factory, string name, string binding = null)
        {
            if (string.IsNullOrEmpty(binding))
            {
                binding = name;
            }

            var textField = factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.TextField);
            var textFieldObj = new DatenMeister.Entities.AsObject.FieldInfo.TextField(textField);
            textFieldObj.setName(name);
            textFieldObj.setBinding(binding);

            if (name == "id")
            {
                textFieldObj.setReadOnly(true);
            }

            return textField;
        }
    }
}
