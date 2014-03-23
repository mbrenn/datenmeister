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

        public CSharpTypeDefinitionFactory(ITypeInfoProvider provider, string nameSpace, string className)
            : base(provider)
        {
            this.nameSpace = nameSpace;
            this.className = className;
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
            writer.WriteLine(string.Format(
                FourSpaces + "public static class {0}",
                this.className));
            writer.WriteLine(FourSpaces + "{");

            var typeProperties = new StringBuilder();
            var assignFunction = new StringBuilder();
            writer.WriteLine(EightSpaces + "public static DatenMeister.IURIExtent Init()");
            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(TwelveSpaces + "var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent(\"datenmeister:///types\");");

            assignFunction.AppendLine(EightSpaces + "public static void AssignTypeMapping(DatenMeister.DataProvider.DotNet.DotNetExtent extent)");
            assignFunction.AppendLine(EightSpaces + "{");

            foreach (var type in provider.GetTypes())
            {
                // Creates property for the type
                typeProperties.AppendFormat(EightSpaces + "public static DatenMeister.IObject {0};", type);
                typeProperties.AppendLine();
                typeProperties.AppendLine();

                // Creates the object instance for the type
                writer.WriteLine(TwelveSpaces + "{");
                writer.WriteLine(SixteenSpaces + "var type = new DatenMeister.Entities.UML.Type();");
                writer.WriteLine(string.Format(SixteenSpaces + "type.name = \"{0}\";", type));
                writer.WriteLine(string.Format(SixteenSpaces + "{1}.{0} = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);", type, this.className));
                writer.WriteLine(string.Format(SixteenSpaces + "extent.Add({1}.{0});", type, this.className));
                writer.WriteLine(TwelveSpaces + "}");
                writer.WriteLine();

                // Performs the assignment
                assignFunction.AppendFormat(
                    TwelveSpaces + "extent.Mapping.Add(typeof({0}), {2}.{1});",
                    this.provider.GetFullTypeName(type),
                    type,
                    this.className);
                assignFunction.AppendLine();
            }

            writer.WriteLine(TwelveSpaces + "return extent;");
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            assignFunction.AppendLine(EightSpaces + "}");

            writer.WriteLine(typeProperties.ToString());
            writer.WriteLine(assignFunction.ToString());

            writer.WriteLine(FourSpaces + "}");
            writer.WriteLine("}");
        }
    }
}
