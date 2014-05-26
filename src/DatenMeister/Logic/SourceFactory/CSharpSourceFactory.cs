using System;
using System.Collections;
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
            writer.WriteLine(FourSpaces + "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"DatenMeister.Logic.SourceFactory.CSharpSourceFactory\", \"1.0.5.0\")]");
            writer.WriteLine(FourSpaces + "[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]");
            writer.WriteLine(
                string.Format(
                    FourSpaces + "public class {0} : DatenMeister.IObject",
                    typeName));

            writer.WriteLine(FourSpaces + "{");

            // Property for the reference to IObject
            writer.WriteLine(EightSpaces + "private DatenMeister.IObject obj;");

            this.EmitConstructor(writer, typeName);

            this.EmitCreationByFactory(writer, typeName);

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

        /// <summary>
        /// Emits the function, which creates an instance by using a factory
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="typeName"></param>
        private void EmitCreationByFactory(StreamWriter writer, string typeName)
        {
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public static IObject create(DatenMeister.IFactory factory)")
                );

            writer.WriteLine(EightSpaces + "{");
            //writer.WriteLine(TwelveSpaces + "throw new System.InvalidOperationException();");
            writer.WriteLine(TwelveSpaces + "return factory.create(" + this.nameSpace + ".Types." + typeName + ");");
            writer.WriteLine(EightSpaces + "}");
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
            var propertyTypeName = this.GetPropertyTypeName(propertyType);

            // Emits Get-Method
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public {1} {0}()",
                    this.GetGetMethodName(propertyName, propertyType),
                    propertyTypeName));
            writer.WriteLine(EightSpaces + "{");
            if (this.HasPushMethod(propertyType))
            {
                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "var result = DatenMeister.Extensions.AsEnumeration(this.get(\"{0}\"));",
                        propertyName));
            }
            else
            {
                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "var result = DatenMeister.Extensions.AsSingle(this.get(\"{0}\"));",
                        propertyName));
            }

            writer.WriteLine(
                string.Format(
                    TwelveSpaces + "return (result is {0}) ? (({0}) result) : default({0});",
                    propertyTypeName));
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            // Emits Set-Method
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public void {0}({1} value)",
                    this.GetSetMethodName(propertyName, propertyType), 
                    propertyTypeName));
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
                        EightSpaces + "public void {0}({1} value)",
                        this.GetPushMethodName(propertyName, propertyType),
                        propertyTypeName));
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

        /// <summary>
        /// Gets the type name as being used in C#-File for a given type
        /// Generally spoken, it just returns the type itself, except the type
        /// is a list, a collection or any other enumeration, implementing IEnumerable. 
        /// In this case, the IEnumerable interface will be returned
        /// </summary>
        /// <param name="type">Type to be used</param>
        /// <returns>Typename as being used in C# file</returns>
        public string GetPropertyTypeName(Type type)
        {
            if (type != typeof(string))
            {
                // Ok, we might have an enumeration, which is a list, a collection or any other type
                // All enumerations will be handled as IEnumerable
                foreach (var ifs in type.GetInterfaces())
                {
                    if (ifs.IsGenericType && (ifs.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    {
                        return string.Format(
                            "System.Collections.Generic.IEnumerable<{0}>",
                            this.GetPropertyTypeName(ifs.GetGenericArguments().First()));
                    }
                }
            }

            return type.ToString();
        }
    }

}
