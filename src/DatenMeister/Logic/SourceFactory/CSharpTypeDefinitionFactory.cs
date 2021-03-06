﻿using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.SourceFactory
{
    /// <summary>
    /// Creates the classes, which contain type definition as IObject elements. 
    /// The elements are stored in a DotNetExtent
    /// </summary>
    public class CSharpTypeDefinitionFactory : SourceFactoryBase
    {
        private string nameSpace;

        private string className;

        private string typeExtentUri;

        public CSharpTypeDefinitionFactory(ITypeInfoProvider provider, string nameSpace, string className, string typeExtentUri)
            : base(provider)
        {
            this.nameSpace = nameSpace;
            this.className = className;
            this.typeExtentUri = typeExtentUri;
        }

        public void CreateFile(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                this.Emit(writer);
            }
        }

        private void Emit(StreamWriter writer)
        {
            writer.WriteLine(string.Format(
                "namespace {0}", this.nameSpace));
            writer.WriteLine("{");
            writer.WriteLine(FourSpaces
                + "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"DatenMeister.Logic.SourceFactory.CSharpTypeDefinitionFactory\", \""
                + CSharpSourceFactory.FactoryVersion.ToString()
                + "\")]");
            writer.WriteLine(FourSpaces + "[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]");
            writer.WriteLine(string.Format(
                FourSpaces + "public static partial class {0}",
                this.className));
            writer.WriteLine(FourSpaces + "{");

            var typeProperties = new StringBuilder();
            var assignFunction = new StringBuilder();
            writer.WriteLine(EightSpaces + "public const string DefaultExtentUri=\"{0}\";", this.typeExtentUri);
            writer.WriteLine();
            writer.WriteLine(EightSpaces + "public static DatenMeister.IURIExtent Init()");
            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(TwelveSpaces + "var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent(DefaultExtentUri);");
            writer.WriteLine(TwelveSpaces + "DatenMeister.Entities.AsObject.Uml.Types.AssignTypeMapping(extent);");
            writer.WriteLine(TwelveSpaces + "Init(extent);");
            writer.WriteLine(TwelveSpaces + "return extent;");
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            writer.WriteLine(EightSpaces + "public static void Init(DatenMeister.IURIExtent extent)");
            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(TwelveSpaces + "var factory = DatenMeister.DataProvider.Factory.GetFor(extent);");

            assignFunction.AppendLine(EightSpaces + "public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)");
            assignFunction.AppendLine(EightSpaces + "{");
            assignFunction.AppendLine(TwelveSpaces + "AssignTypeMapping(extent.Mapping);");
            assignFunction.AppendLine(EightSpaces + "}");
            assignFunction.AppendLine();
            assignFunction.AppendLine(EightSpaces + "public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.IMapsMetaClassFromDotNet mapping)");
            assignFunction.AppendLine(EightSpaces + "{");

            var propertyAssignments = new StringBuilder();
            foreach (var type in provider.GetTypes())
            {
                // Creates property for the type
                typeProperties.AppendFormat(EightSpaces + "public static DatenMeister.IObject {0};", type);
                typeProperties.AppendLine();
                typeProperties.AppendLine();

                // Creates the object instance for the type
                writer.WriteLine(TwelveSpaces + "if({1}.{0} == null || true)", type, this.className);
                writer.WriteLine(TwelveSpaces + "{");
                writer.WriteLine(string.Format(SixteenSpaces + "{1}.{0} = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);", type, this.className));
                writer.WriteLine(string.Format(SixteenSpaces + "DatenMeister.Entities.AsObject.Uml.Type.setName({1}.{0}, \"{0}\");", type, this.className));
                writer.WriteLine(string.Format(SixteenSpaces + "extent.Elements().add({1}.{0});", type, this.className));
                writer.WriteLine(TwelveSpaces + "}");
                writer.WriteLine();

                // Assigns the properties
                foreach (var property in provider.GetProperties(type))
                {
                    propertyAssignments.AppendLine();
                    
                    propertyAssignments.AppendLine(TwelveSpaces + "{");
                    propertyAssignments.AppendLine(string.Format(SixteenSpaces + "// {0}.{1}", type, property));
                    propertyAssignments.AppendLine(string.Format(SixteenSpaces + "var property = factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);"));
                    propertyAssignments.AppendLine(string.Format(SixteenSpaces + "DatenMeister.Entities.AsObject.Uml.Property.setName(property, \"{0}\");", property));
                    propertyAssignments.AppendLine(string.Format(SixteenSpaces + "DatenMeister.Entities.AsObject.Uml.Class.pushOwnedAttribute({1}.{0}, property);", type, this.className));
                    propertyAssignments.AppendLine(TwelveSpaces + "}");
                }

                // Performs the assignment
                assignFunction.AppendFormat(
                    TwelveSpaces + "mapping.Add(typeof({0}), {2}.{1});",
                    this.provider.GetFullTypeName(type),
                    type,
                    this.className);
                assignFunction.AppendLine();
            }

            writer.WriteLine();

            writer.WriteLine(TwelveSpaces + "if(extent is DatenMeister.DataProvider.DotNet.DotNetExtent)");
            writer.WriteLine(TwelveSpaces + "{");
            writer.WriteLine(SixteenSpaces + "(extent as DatenMeister.DataProvider.DotNet.DotNetExtent).AddDefaultMappings();");
            writer.WriteLine(TwelveSpaces + "}");
            writer.WriteLine();
            writer.WriteLine(TwelveSpaces + "OnInitCompleted();");
            writer.WriteLine(propertyAssignments.ToString());
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            assignFunction.AppendLine(EightSpaces + "}");

            writer.WriteLine(typeProperties.ToString());
            writer.WriteLine(assignFunction.ToString());
            writer.WriteLine(EightSpaces + "static partial void OnInitCompleted();");


            writer.WriteLine(FourSpaces + "}");
            writer.WriteLine("}");
        }
    }
}
