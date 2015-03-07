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
        public static Version FactoryVersion = new Version(1, 1, 0, 0);

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

        /// <summary>
        /// Emits the definition for the complete type. 
        /// It include the class, the property get and setters and the static access properties
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="typeName"></param>
        private void EmitType(StreamWriter writer, string typeName)
        {
            writer.WriteLine(FourSpaces 
                + "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"DatenMeister.Logic.SourceFactory.CSharpSourceFactory\", \"" 
                + FactoryVersion.ToString() 
                + "\")]");
            writer.WriteLine(FourSpaces + "[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]");
            writer.WriteLine(
                string.Format(
                    FourSpaces + "public class {0} : DatenMeister.IObject, DatenMeister.DataProvider.IProxyObject",
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
                this.EmitGetSetForProperty(writer, typeName, propertyName);

                this.EmitStaticGetSetForProperty(writer, typeName, propertyName);
            }

            // End of class
            writer.WriteLine(FourSpaces + "}");
            writer.WriteLine();
        }

        /// <summary>
        /// Emits the function, which creates an instance by using a factory.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="typeName">Type name, for whom the factory shall be created</param>
        private void EmitCreationByFactory(StreamWriter writer, string typeName)
        {
            //////////////////////////
            // Writes the generic creation method
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public static DatenMeister.IObject create(DatenMeister.IFactory factory)")
                );

            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(TwelveSpaces + "return factory.create(" + this.nameSpace + ".Types." + typeName + ");");
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            ///////////////////////////////
            // Writes the typed creation method
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public static {0} createTyped(DatenMeister.IFactory factory)",
                    typeName)
                );

            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(TwelveSpaces + "return new {0}(create(factory));", typeName);
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
        private void EmitGetSetForProperty(StreamWriter writer, string typeName, string propertyName)
        {
            var propertyType = this.provider.GetTypeOfProperty(typeName, propertyName);
            var propertyTypeName = this.GetPropertyTypeName(propertyType);

            // Emits Get-Method
            var getMethodName = this.GetGetMethodName(propertyName, propertyType);
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public {1} {0}()",
                    getMethodName,
                    propertyTypeName));
            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(
                string.Format(
                    TwelveSpaces + "return {0}(this);",
                    getMethodName));
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            // Emits Set-Method
            var setMethodName = this.GetSetMethodName(propertyName, propertyType);
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public void {0}({1} value)",
                    setMethodName, 
                    propertyTypeName));
            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(
                string.Format(
                    TwelveSpaces + "{0}(this, value);",
                    setMethodName));
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            // Emits push method
            if (this.HasPushMethod(propertyType))
            {
                var itemOfListProperty = propertyType.GenericTypeArguments.First();
                var itemOfListPropertyName = this.GetPropertyTypeName(itemOfListProperty);

                var pushMethodName = this.GetPushMethodName(propertyName, propertyType);
                writer.WriteLine(
                    string.Format(
                        EightSpaces + "public void {0}({1} value)",
                        pushMethodName,
                        itemOfListPropertyName));
                writer.WriteLine(EightSpaces + "{");
                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "{0}(this, value);",
                        pushMethodName));

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
        private void EmitStaticGetSetForProperty(StreamWriter writer, string typeName, string propertyName)
        {
            var propertyType = this.provider.GetTypeOfProperty(typeName, propertyName);
            var propertyTypeName = this.GetPropertyTypeName(propertyType);

            // Emits Get-Method
            var getMethodName = this.GetGetMethodName(propertyName, propertyType);
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public static {1} {0}(DatenMeister.IObject obj)",
                    getMethodName,
                    propertyTypeName));
            writer.WriteLine(EightSpaces + "{");
            if (this.HasPushMethod(propertyType))
            {
                var typeOfListProperty = propertyType.GenericTypeArguments.First();
                var typeOfListPropertyName = this.GetPropertyTypeName(typeOfListProperty);

                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "var result = DatenMeister.Extensions.getAsReflectiveSequence(obj, \"{0}\");",
                        propertyName,
                        typeOfListPropertyName));
            }
            else
            {
                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "var result = obj.get(\"{0}\", DatenMeister.RequestType.AsSingle);",
                        propertyName));
            }

            // Checks, if the property type is a special type
            if (propertyType == typeof(bool))
            {
                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "return DatenMeister.ObjectConversion.ToBoolean(result);"));
            }
            else if (propertyType == typeof(string))
            {
                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "return DatenMeister.ObjectConversion.ToString(result);"));
            }
            else if (propertyType == typeof(int))
            {
                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "return DatenMeister.ObjectConversion.ToInt32(result);"));
            }
            else if (ObjectConversion.IsEnumByType(propertyType))
            {
                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "return ({0}) DatenMeister.ObjectConversion.ConvertToEnum(result, typeof({0}));",
                        propertyTypeName));
            }
            else if (propertyType == typeof(IObject))
            {
                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "return DatenMeister.Extensions.AsIObject(result);"));
            }
            else
            {
                writer.WriteLine(
                    string.Format(
                        TwelveSpaces + "return (result is {0}) ? (({0}) result) : default({0});",
                        propertyTypeName));
            }

            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            // Emits Set-Method
            var setMethodName = this.GetSetMethodName(propertyName, propertyType);
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public static void {0}(DatenMeister.IObject obj, {1} value)",
                    setMethodName,
                    propertyTypeName));
            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(
                string.Format(
                    TwelveSpaces + "obj.set(\"{0}\", value);",
                    propertyName));
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            // Emits push method
            if (this.HasPushMethod(propertyType))
            {
                if (propertyType != typeof(IList<IObject>))
                {
                    this.CreatePushMethod(writer, propertyName, typeof(IList<IObject>));
                }

                this.CreatePushMethod(writer, propertyName, propertyType);
            }
        }

        /// <summary>
        /// Creates the push method for the given property type and property name
        /// </summary>
        /// <param name="writer">Writer to be used</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="propertyType">Tpye of the property, that can be added to the list</param>
        private void CreatePushMethod(StreamWriter writer, string propertyName, Type propertyType)
        {
            var typeOfListProperty = propertyType.GenericTypeArguments.First();
            var typeOfListPropertyName = this.GetPropertyTypeName(typeOfListProperty);

            writer.WriteLine(
                string.Format(
                    EightSpaces + "public static void {0}(DatenMeister.IObject obj, {1} value)",
                    this.GetPushMethodName(propertyName, propertyType),
                    typeOfListProperty));
            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(
                string.Format(
                    TwelveSpaces + "var list = DatenMeister.Extensions.getAsReflectiveSequence(obj, \"{0}\");",
                    propertyName));
            writer.WriteLine(TwelveSpaces + "list.Add(value);");
            // If we already receive a reflective collection, than the resetting is not necessary
            /*writer.WriteLine(
                string.Format(
                    TwelveSpaces + "obj.set(\"{0}\", list);",
                    propertyName));*/

            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();
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
            // Type of string needs to be skipped, it is an enumeration, but we do not
            // want to offer strings as enums
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
