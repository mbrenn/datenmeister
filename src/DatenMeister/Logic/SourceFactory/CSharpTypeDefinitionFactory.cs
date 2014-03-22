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

            var types = new StringBuilder();
            writer.WriteLine(EightSpaces + "public static DatenMeister.IURIExtent Init()");
            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine ( TwelveSpaces + "var extent = new DatenMeister.DataProvider.DotNet.DotNetExtent(\"datenmeister:///types\");");

            foreach (var type in provider.GetTypes())
            {
                types.AppendFormat(EightSpaces + "public static DatenMeister.IObject {0};", type);
                types.AppendLine();
                types.AppendLine();

                writer.WriteLine(TwelveSpaces + "{");
                writer.WriteLine(SixteenSpaces + "var type = new DatenMeister.Entities.UML.Type();");
                writer.WriteLine(string.Format(SixteenSpaces + "type.name = \"{0}\";", type));
                writer.WriteLine(string.Format(SixteenSpaces + "{1}.{0} = new DatenMeister.DataProvider.DotNet.DotNetObject(extent, type);", type, this.className));
                writer.WriteLine(string.Format(SixteenSpaces + "extent.Add({1}.{0});", type, this.className));
                writer.WriteLine(TwelveSpaces + "}");
                writer.WriteLine();
            }

            writer.WriteLine(TwelveSpaces + "return extent;");
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            writer.WriteLine(types.ToString());

            writer.WriteLine(FourSpaces + "}");
            writer.WriteLine("}");
        }
    }
}
