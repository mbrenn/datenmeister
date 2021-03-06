﻿using BurnSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.SourceFactory
{
    /// <summary>
    /// This factory is able to create a type script file, containing
    /// </summary>    
    public class TypeScriptSourceFactory : SourceFactoryBase
    {

        /// <summary>
        /// Defines the parent class of the created types
        /// </summary>
        private string parentClass = "Backbone.Model";

        /// <summary>
        /// Defines the file to be imported for the parent class
        /// </summary>
        private string importedFile = string.Empty;

        /// <summary>
        /// Initializes a new instance of the TypeScriptSource Factory
        /// </summary>
        /// <param name="provider">Provider to be used</param>
        public TypeScriptSourceFactory(ITypeInfoProvider provider)
            : base(provider)
        {
        }

        /// <summary>
        /// Sets the parent class from another import file
        /// </summary>
        /// <param name="parentClass">Name of the class being used for parent class. Shall not include the prefix for 
        /// the imported module</param>
        /// <param name="importFile">Name of the file that will be imported</param>
        public void SetParentClass(string parentClass, string importFile)
        {
            this.parentClass = parentClass;
            this.importedFile = importFile;
        }

        public void CreateFile(string path)
        {
            using (var writer = new StreamWriter(path))
            {
                this.Emit(writer);
            }
        }

        /// <summary>
        /// Creates the file
        /// </summary>
        /// <param name="stream">Stream being used</param>
        public void Emit(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                this.Emit(writer);
            }
        }

        private void Emit(StreamWriter writer)
        {
            // Writes the reference to backbone.js
            writer.WriteLine("/// <reference path=\"../backbone/backbone.d.ts\" />");
            if (!string.IsNullOrEmpty(this.importedFile))
            {
                writer.WriteLine(
                    string.Format(
                        "import __d__ = require('{0}');",
                        this.importedFile));
                this.parentClass = "__d__." + this.parentClass;
            }

            writer.WriteLine(string.Empty);

            foreach (var type in this.provider.GetTypes())
            {
                this.EmitType(writer, type);
            }
        }

        /// <summary>
        /// Creates a typescript class for a specific type
        /// </summary>
        /// <param name="writer"></param>
        private void EmitType(StreamWriter writer, string typeName)
        {
            // Starts the module
            writer.WriteLine(
                string.Format("export module {0} {1}", typeName, "{"));

            // Sets the typename
            writer.WriteLine(
                string.Format(
                    FourSpaces + "export var TypeName='{0}';",
                    typeName));
            writer.WriteLine();

            // Creates the constructor if necessary
            this.EmitConstructor(writer, typeName);

            // Creates the property access methods
            foreach (var propertyName in this.provider.GetProperties(typeName))
            {
                this.EmitProperty(writer, typeName, propertyName);
            }

            // Ends the module
            writer.WriteLine("}");
            writer.WriteLine(string.Empty); 
        }

        /// <summary>
        /// Creates the constructor for the typescript class. 
        /// All arguments are optional
        /// </summary>
        /// <param name="writer">Streamwriter to be used</param>
        /// <param name="typeName">Name of the types</param>
        private void EmitConstructor(StreamWriter writer, string typeName)
        {
            var arguments = this.provider.GetArgumentsForConstructor(typeName);

            // Create the constructor function header
            var constructorLine = new StringBuilder(FourSpaces + "export function create(");

            var first = true;
            foreach (var argument in arguments)
            {
                if (!first)
                {
                    constructorLine.Append(", ");
                }

                constructorLine.AppendFormat("{0}?: any", argument);
                first = false;
            }

            constructorLine.AppendLine(") {");

            // Creates the assignments
            constructorLine.AppendLine(
                string.Format(
                    EightSpaces + "var result = new {0}();",
                    this.parentClass));
            constructorLine.AppendLine(
                string.Format(
                    EightSpaces + "result.set('type', '{0}');",
                    typeName)); 
            
            // Creates the assignments by default values
            foreach (var propertyName in this.provider.GetProperties(typeName))
            {
                var defaultValue = this.provider.GetDefaultValueForProperty(typeName, propertyName);
                if (defaultValue == null)
                {
                    continue;
                }

                // Ok, we have a value, now create
                constructorLine.AppendLine(
                    string.Format(
                        EightSpaces + "result.set('{0}', {1});",
                        propertyName,
                        TypeScriptBuilder.ConvertToLiteral(defaultValue)));
            }

            // Creates the assignments by the constructor
            EmitAssignmentsForConstructor(arguments, constructorLine);

            // Returns
            constructorLine.AppendLine(EightSpaces + "return result;");
            constructorLine.AppendLine(FourSpaces + "}");

            writer.WriteLine(constructorLine);
            writer.WriteLine();
        }

        /// <summary>
        /// Builds the assignments for the constructor
        /// </summary>
        /// <param name="arguments">List of arguments being assigned</param>
        /// <param name="constructorLine">StringBuilder being used to create the construction</param>
        private static void EmitAssignmentsForConstructor(List<string> arguments, StringBuilder constructorLine)
        {
            foreach (var argument in arguments)
            {
                constructorLine.AppendLine(
                    string.Format(
                        EightSpaces + "if ({0} !== undefined) {1}",
                        argument,
                        "{"));
                constructorLine.AppendLine(
                    string.Format(
                        FourSpaces + EightSpaces + "result.set('{0}', {0});",
                        argument));
                constructorLine.AppendLine(
                    EightSpaces + "}");
                constructorLine.AppendLine();
            }
        }

        /// <summary>
        /// Creates a typescript property for a specfic property in the type
        /// </summary>
        /// <param name="writer">Writer being used</param>
        /// <param name="typeName">Name of the type</param>
        /// <param name="propertyName">Name of the property</param>
        private void EmitProperty(StreamWriter writer, string typeName, string propertyName)
        {
            var propertyType = this.provider.GetTypeOfProperty(typeName, propertyName);

            this.EmitGetMethod(writer, propertyName, propertyType);

            this.EmitSetMethod(writer, propertyName, propertyType);

            if (this.HasPushMethod(propertyType))
            {
                this.EmitPushMethod(writer, propertyName, propertyType);
            }
        }

        private void EmitGetMethod(StreamWriter writer, string propertyName, Type propertyType)
        {
            ///////////////////////////////
            // Creates the get method

            // Check, if propertyname needs to get transformed
            var functionName = this.GetGetMethodName(propertyName, propertyType);

            // get{Property}(item: {this.parentClass}) {
            writer.WriteLine(
                string.Format(
                    FourSpaces + "export function {0}(item: {1}) {2}",
                    functionName,
                    this.parentClass,
                    "{"));

            // return item.get('{propertyName}');
            writer.WriteLine(string.Format(EightSpaces + "return item.get('{0}');", propertyName));

            // }
            writer.WriteLine(FourSpaces + "}");
            writer.WriteLine();
        }

        private void EmitSetMethod(StreamWriter writer, string propertyName, Type propertyType)
        {
            ///////////////////////////////
            // Creates the set method
            // Check, if propertyname needs to get transformed
            var functionName = this.GetSetMethodName(propertyName, propertyType);

            // set{Property}(item: {this.parentClass}, value: any) {
            writer.WriteLine(
                string.Format(
                    FourSpaces + "export function {0}(item : {1}, value: any) {2}",
                    functionName,
                    this.parentClass,
                    "{"));

            // return item.get('{propertyName}');
            writer.WriteLine(string.Format(EightSpaces + "item.set('{0}', value);", propertyName));

            // }
            writer.WriteLine(FourSpaces + "}");
            writer.WriteLine();
        }

        private void EmitPushMethod(StreamWriter writer, string propertyName, Type propertyType)
        {
            ///////////////////////////////
            // Creates the push method
            var functionName = this.GetPushMethodName(propertyName, propertyType);

            // push{Property}(item: {this.parentClass}, value: any) {
            writer.WriteLine(
                string.Format(
                    FourSpaces + "export function {0}(item : {1}, value: any) {2}",
                    functionName,
                    this.parentClass,
                    "{"));

            // var a = <Array<{this.parentClass}>> item.get('{propertyName}');
            writer.WriteLine(
                string.Format(
                    EightSpaces + "var a = <Array<any>> item.get('{1}');",
                    this.parentClass,
                    propertyName
                ));
            // if (a == undefined) {
            writer.WriteLine(EightSpaces + "if (a === undefined) {");
            //     a = new Array<{this.parentClass}>();
            writer.WriteLine(
                string.Format(
                    FourSpaces + EightSpaces + "a = new Array<any>();",
                    this.parentClass));
            // }
            writer.WriteLine(EightSpaces + "}");

            // 
            writer.WriteLine();
            // a.push(value);
            writer.WriteLine(EightSpaces + "a.push(value);");
            // item.set('{propertyName}'
            writer.WriteLine(
                string.Format(
                    EightSpaces + "item.set('{0}', a);",
                    propertyName));

            // }
            writer.WriteLine(FourSpaces + "}");
            writer.WriteLine();
        }
    }
}
