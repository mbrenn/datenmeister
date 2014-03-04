using BurnSystems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.SourceFactory
{
    /// <summary>
    /// This factory is able to create a type script file, containing
    /// </summary>    
    public class TypeScriptSourceFactory
    {
        /// <summary>
        /// Contains four spaces
        /// </summary>
        private static string FourSpaces = "    ";

        /// <summary>
        /// Contains eight spaces
        /// </summary>
        private static string EightSpaces = "        ";

        /// <summary>
        /// Stores the provider
        /// </summary>
        private ITypeInfoProvider provider;

        /// <summary>
        /// Defines the parent class of the created types
        /// </summary>
        private string parentClass = "Backbone.Model";

        /// <summary>
        /// Initializes a new instance of the TypeScriptSource Factory
        /// </summary>
        /// <param name="provider">Provider to be used</param>
        public TypeScriptSourceFactory(ITypeInfoProvider provider)
        {
            this.provider = provider;
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
            writer.WriteLine(string.Empty);

            foreach (var type in this.provider.GetTypes())
            {
                this.Emit(writer, type);
            }
        }

        /// <summary>
        /// Creates a typescript class for a specific type
        /// </summary>
        /// <param name="writer"></param>
        private void Emit(StreamWriter writer, string type)
        {
            // Starts the module
            writer.WriteLine(
                string.Format("export module {0} {1}", type, "{"));

            // Creates the 'create' method
            writer.WriteLine(
                string.Format(
                    FourSpaces + "export function create(): {0} {1}",
                    this.parentClass,
                    "{"));

            writer.WriteLine(
                string.Format(
                    EightSpaces + "var result = new {0}()",
                    this.parentClass));
            writer.WriteLine(EightSpaces + "return result;");
            writer.WriteLine(FourSpaces + "}");
            writer.WriteLine();

            // Creates the property access methods
            foreach (var property in this.provider.GetProperties(type))
            {
                this.CreateProperty(writer, type, property);
            }

            // Ends the module
            writer.WriteLine("}");
            writer.WriteLine(string.Empty); 
        }

        /// <summary>
        /// Creates a typescript property for a specfic property in the type
        /// </summary>
        /// <param name="writer">Writer being used</param>
        /// <param name="type">Name of the type</param>
        /// <param name="propertyName">Name of the property</param>
        private void CreateProperty(StreamWriter writer, string type, string propertyName)
        {
            ///////////////////////////////
            // Creates the get method

            // get{Property}(item: {this.parentClass}) {
            writer.WriteLine(
                string.Format(
                    FourSpaces + "export function get{0}(item : {1}) {2}", 
                    StringManipulation.ToUpperFirstLetter(propertyName), 
                    this.parentClass, 
                    "{"));

            // return item.get('{propertyName}');
            writer.WriteLine(string.Format(EightSpaces + "return item.get('{0}');", propertyName));

            // }
            writer.WriteLine(FourSpaces + "}");
            writer.WriteLine();

            ///////////////////////////////
            // Creates the set method

            // set{Property}(item: {this.parentClass}, value: any) {
            writer.WriteLine(
                string.Format(
                    FourSpaces + "export function set{0}(item : {1}, value: any) {2}", 
                    StringManipulation.ToUpperFirstLetter(propertyName), 
                    this.parentClass, 
                    "{"));

            // return item.get('{propertyName}');
            writer.WriteLine(string.Format(EightSpaces + "item.set('{0}', value);", propertyName));

            // }
            writer.WriteLine(FourSpaces + "}");
            writer.WriteLine();

        }
    }
}
