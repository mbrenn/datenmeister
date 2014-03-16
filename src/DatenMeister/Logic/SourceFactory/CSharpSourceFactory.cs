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
    public class CSharpSourceFactory
    {
        /// <summary>
        /// Stores the provider
        /// </summary>
        private ITypeInfoProvider provider;

        /// <summary>
        /// Namespace to be used for the class
        /// </summary>
        private string nameSpace;

        /// <summary>
        /// Contains four spaces
        /// </summary>
        internal static string FourSpaces
        {
            get { return TypeScriptSourceFactory.FourSpaces; }
        }

        /// <summary>
        /// Contains eight spaces
        /// </summary>
        internal static string EightSpaces
        {
            get { return TypeScriptSourceFactory.EightSpaces; }
        }

        /// <summary>
        /// Contains twelve spaces
        /// </summary>
        internal static string TwelveSpaces
        {
            get { return EightSpaces + FourSpaces; }
        }

        /// <summary>
        /// Initializes a new instance of the CSharpSourceFactory class.
        /// </summary>
        /// <param name="provider">Provder to be used</param>
        public CSharpSourceFactory(ITypeInfoProvider provider, string nameSpace)
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

        private void EmitType(StreamWriter writer, string type)
        {
            writer.WriteLine(
                string.Format(
                    FourSpaces + "public class {0} : DatenMeister.IObject",
                    type));

            writer.WriteLine(FourSpaces + "{");

            // Property for the reference to IObject
            writer.WriteLine(EightSpaces + "private DatenMeister.IObject obj;");

            // Most simple Constructor for the object
            writer.WriteLine(
                string.Format(
                    EightSpaces + "public {0}(DatenMeister.IObject obj)",
                    type));
            writer.WriteLine(EightSpaces + "{");
            writer.WriteLine(TwelveSpaces + "this.obj = obj");
            writer.WriteLine(EightSpaces + "}");
            writer.WriteLine();

            // Implements the IObject interface
            writer.WriteLine(Resources_DatenMeister.IObjectImplementation);

            // End of class
            writer.WriteLine(FourSpaces + "}");
            writer.WriteLine();
        }
    }

    
}
