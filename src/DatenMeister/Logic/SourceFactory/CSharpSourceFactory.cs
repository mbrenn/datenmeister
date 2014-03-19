using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.SourceFactory
{
    /// <summary>
    /// Creates a c-sharp file out of the Type Provider. 
    /// The C-Sharp file contains getter and setter to access the properties view IObject-Interface
    /// </summary>
    public class CSharpSourceFactory : SourceFactoryBase
    {
        /// <summary>
        /// Namespace to be used for the class
        /// </summary>
        private string nameSpace;

        /// <summary>
        /// Initializes a new instance of the CSharpSourceFactory class.
        /// </summary>
        /// <param name="provider">Provder to be used</param>
        public CSharpSourceFactory(ITypeInfoProvider provider, string nameSpace)
            : base(provider)
        {
            this.provider = provider;
            this.nameSpace = nameSpace;
        }

        /// <summary>
        /// Creates one c# at the given location. 
        /// The file contains all classes.
        /// </summary>
        /// <param name="path">Path to be used</param>
        public void CreateFile(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                this.Emit(writer);
            }
        }

        /// <summary>
        /// Emits the file into the writer
        /// </summary>
        /// <param name="writer">Writer to be used</param>
        private void Emit(StreamWriter writer)
        {
            writer.WriteLine(string.Format(
                "namespace {0}", this.nameSpace));
            writer.WriteLine("{");

            foreach (var type in this.provider.GetTypes())
            {
                this.EmitType(writer, type);
            }

            writer.WriteLine("}");
        }

        private void EmitType(StreamWriter writer, string typeName)
        {
            writer.WriteLine(
                string.Format(
                    FourSpaces + "public class {0} : DatenMeister.IObject",
                    typeName));

            writer.WriteLine(FourSpaces + "{");

            // Property for the reference to IObject
            writer.WriteLine(EightSpaces + "private DatenMeister.IObject obj;");

            this.EmitConstructor(writer, typeName);

            // Implements the IObject interface
            writer.WriteLine(Resources_DatenMeister.IObjectImplementation);

            // Now write all the properties
            foreach (var propertyName in this.provider.GetProperties(typeName))
            {
                this.EmitProperty(writer, typeName, propertyName);
            }

            // End of class
            writer.WriteLine(FourSpaces + "}");
            writer.WriteLine();
        }

        private void EmitConstructor(StreamWriter writer, string typeName)
        {

            // Most simple Constructor for the object
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public {0}(DatenMeister.IObject obj)",
                    typeName));
            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(TwelveSpaces + "this.obj = obj;");
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            // Emits all other constructors
            var constructorArguments = this.provider.GetArgumentsForConstructor(typeName);
            if (constructorArguments != null && constructorArguments.Count != 0)
            {
                writer.Write(
                    string.Format(
                        EightSpaces + "public {0}(DatenMeister.IObject obj",
                        typeName));

                foreach (var argument in constructorArguments)
                {
                    writer.Write(string.Format(", object {0}", argument));
                }

                writer.WriteLine(")");
                writer.WriteLine(TwelveSpaces + ": this(obj)");
                writer.WriteLine(EightSpaces + "{");
                foreach (var argument in constructorArguments)
                {
                    writer.WriteLine(
                        string.Format(
                            TwelveSpaces + "this.set(\"{0}\", {0});",
                            argument));
                }

                writer.WriteLine(EightSpaces + "}");
                writer.WriteLine();
            }
        }

        /// <summary>
        /// Emits the property for the given type
        /// </summary>
        /// <param name="writer">Writer to be used</param>
        /// <param name="typeName">Type to be used</param>
        /// <param name="propertyName">Property to be emitted</param>
        private void EmitProperty(StreamWriter writer, string typeName, string propertyName)
        {
            var propertyType = this.provider.GetTypeOfProperty(typeName, propertyName);

            // Emits Get-Method
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public object {0}()",
                    this.GetGetMethodName(propertyName, propertyType)));
            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(
                string.Format(
                    TwelveSpaces + "return this.get(\"{0}\");",
                    propertyName));
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            // Emits Set-Method
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public void {0}(object value)",
                    this.GetSetMethodName(propertyName, propertyType)));
            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(
                string.Format(
                    TwelveSpaces + "this.set(\"{0}\", value);",
                    propertyName));
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            // Emits push method
            if (this.HasPushMethod(propertyType))
            {
                writer.WriteLine(
                    string.Format(
                        EightSpaces + "public void {0}(object value)",
                        this.GetPushMethodName(propertyName, propertyType)));
                writer.WriteLine(EightSpaces + "{");
                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "var list = this.get(\"{0}\") as System.Collections.IList ?? new System.Collections.Generic.List<object>();",
                        propertyName));
                writer.WriteLine(TwelveSpaces + "list.Add(value);");
                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "this.set(\"{0}\", list);",
                        propertyName));

                writer.WriteLine(EightSpaces + "}");
                writer.WriteLine();
            }
        }
    }

}
