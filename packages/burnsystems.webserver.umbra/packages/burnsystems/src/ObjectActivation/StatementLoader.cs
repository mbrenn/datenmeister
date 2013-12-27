using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BurnSystems.Logging;
using Microsoft.CSharp;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Loads a C#-Sourcecode file, compiles it and calls the static methods defined in C#-File
    /// </summary>
    public class StatementLoader
    {
        /// <summary>
        /// Defines the class logger to be used
        /// </summary>
        private ClassLogger classLogger = new ClassLogger(typeof(StatementLoader));

        /// <summary>
        /// Stores the container
        /// </summary>
        private ActivationContainer Container
        {
            get;
            set;
        }

        /// <summary>
        /// Loads and executes statements from file
        /// </summary>
        /// <param name="container">Container being used</param>
        /// <param name="path">Path to be loaded</param>
        public static void FromFile(ActivationContainer container, string path)
        {
            var loader = new StatementLoader(container);
            loader.LoadAndExecute(File.ReadAllText(path));
        }

        /// <summary>
        /// Initializes a new instnace of the StatementLoader.
        /// </summary>
        /// <param name="container">Container to be loaded</param>
        public StatementLoader(ActivationContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// Loads and executes the configuration file
        /// </summary>
        /// <param name="csFile">File to be executed</param>
        public void LoadAndExecute(string csFile)
        {
            var results = this.Compile(csFile);

            // Check for errors
            this.CheckForErrors(results);

            this.InvokeStaticMethods(results);
        }

        /// <summary>
        /// Compiles the csfile
        /// </summary>
        /// <param name="csFile">CS File to be compiled</param>
        /// <returns>Compiler results</returns>
        private CompilerResults Compile(string csFile)
        {
            var csc = new CSharpCodeProvider();

            var parameters = new CompilerParameters(this.GetAssemblies());
            parameters.GenerateInMemory = true;
            var results = csc.CompileAssemblyFromSource(
                parameters,
                new[] { csFile });
            return results;
        }

        /// <summary>
        /// Checks if errors have occured
        /// </summary>
        /// <param name="result">Results of the compilation</param>
        private void CheckForErrors(CompilerResults result)
        {
            var errors = result.Errors.Cast<CompilerError>().ToList();
            var errorText = new StringBuilder();
            foreach (var error in errors)
            {
                classLogger.LogEntry(new LogEntry(error.ErrorText, LogLevel.Fatal));
                errorText.AppendLine(error.ToString());
            }

            if (errors.Count > 0)
            {
                throw new InvalidOperationException(errors.Count + " error(s) occured during compilation: " + errorText.ToString());
            }
        }

        /// <summary>
        /// Invokes all static methods of the compilation result
        /// </summary>
        /// <param name="result">Result of the compilation</param>
        private void InvokeStaticMethods(CompilerResults result)
        {
            foreach (var type in result.CompiledAssembly.GetTypes())
            {
                foreach (var method in type.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
                {
                    var methodParameters = method.GetParameters();
                    if (methodParameters.Length == 1
                        && methodParameters[0].ParameterType == typeof(ActivationContainer))
                    {
                        method.Invoke(null, new object[] { this.Container });
                    }
                }
            }
        }

        /// <summary>
        /// Gets the list of assemblies
        /// </summary>
        /// <returns>List of assemblies</returns>
        private string[] GetAssemblies()
        {
            var result = new List<string>();
            result.Add("mscorlib.dll");
            result.Add("System.Core.dll");
            result.Add("System.Data.dll");
            result.Add("System.Xml.dll");
            result.Add("System.Xml.Linq.dll");

            var files = Directory.GetFiles(".", "*.dll");
            result.AddRange(files);

            return result.ToArray();
        }
    }
}
