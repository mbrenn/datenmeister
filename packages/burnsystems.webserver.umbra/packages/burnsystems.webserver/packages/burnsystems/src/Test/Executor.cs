//-----------------------------------------------------------------------
// <copyright file="Executor.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Remoting;

    /// <summary>
    /// This class collects the testcase methods and offer methods
    /// to start the testing. 
    /// </summary>
    public class Executor
    {
        /// <summary>
        /// List of methods with testcases
        /// </summary>
        private List<MethodBase> methods =
            new List<MethodBase>();

        /// <summary>
        /// Gets a list of methods with testcases
        /// </summary>
        public List<MethodBase> Methods
        {
            get { return this.methods; }
        }

        /// <summary>
        /// Calls a method and returns result of testcase
        /// </summary>
        /// <param name="method">Method with testcase</param>
        /// <returns>Result of testcase</returns>
        public static Result TestMethod(MethodBase method)
        {
            Ensure.IsNotNull(method);

            if (!method.IsPublic)
            {
                // Nur öffentliche Methoden
                throw new InvalidOperationException(
                    LocalizationBS.Executor_NoPublicMethod);
            }

            if (method.GetParameters().Length != 0)
            {
                // Nur Methoden ohne einem Parameter können aufgerufen
                // werden
                throw new InvalidOperationException(
                    LocalizationBS.Executor_InvalidParameterCount);
            }

            // Erzeugt die AppDomain
            var appDomain = AppDomain.CreateDomain(
                "TESTDomain",
                null,
                Environment.CurrentDirectory,
                Environment.CurrentDirectory,
                false);

            var typeObject = appDomain.CreateInstanceFrom(
                method.ReflectedType.Assembly.Location,
                method.ReflectedType.FullName);

            // Startet nun den Test
            var result = new Result();
            Stopwatch watch = new Stopwatch();
            try
            {
                watch.Start();

                appDomain.DoCallBack(new Helper(typeObject, method).Invoke);

                result.Failed = false;
            }
            catch (Exception exc)
            {
                result.Failed = true;
                result.Exception = exc;
            }
            finally
            {
                watch.Stop();
                result.Duration = watch.Elapsed;
            }

            // Entlädt die AppDomain
            AppDomain.Unload(appDomain);
            return result;
        }

        /// <summary>
        /// Loads all testcases within assembly
        /// </summary>
        /// <param name="assembly">Assembly with tests</param>
        public void LoadAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var attributes = type.GetCustomAttributes(
                    typeof(TestClassAttribute), false);

                if (attributes.Length == 0)
                {
                    // Kein Attribut, kein Test
                    continue;
                }

                if (!type.IsPublic)
                {
                    throw new InvalidOperationException(
                        LocalizationBS.Executor_NoPublicClass);
                }

                foreach (var method in type.GetMethods())
                {
                    var methodAttributes = method.GetCustomAttributes(
                        typeof(TestMethodAttribute), false);
                    if (methodAttributes.Length == 0)
                    {
                        continue;
                    }                 

                    this.methods.Add(method);
                }
            }
        }

        /// <summary>
        /// Loads an assembly in path. If assembly is already loaded, 
        /// it will be reused
        /// </summary>
        /// <param name="path">Path to assembly</param>
        public void LoadAssembly(string path)
        {
            path = Path.GetFullPath(path);

            // Suche zuerst die schon geladenen Assemblies durch
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var assemblyPath = Path.GetFullPath(
                    assembly.Location);
                if (assemblyPath == path)
                {
                    this.LoadAssembly(assembly);
                    return;
                }
            }

            // Nun lade die Assembly
            var loadedAssembly = Assembly.LoadFile(path);
            this.LoadAssembly(loadedAssembly);
        }

        /// <summary>
        /// A helperclass for invoking the testcase
        /// </summary>
        [Serializable()]
        internal class Helper
        {
            /// <summary>
            /// Methodinformation with testcase
            /// </summary>
            private MethodBase methodBase;

            /// <summary>
            /// Object, being instantiated for called by methodbase
            /// </summary>
            private ObjectHandle handle;

            /// <summary>
            /// Initializes a new instance of the Helper class. 
            /// </summary>
            /// <param name="handle">ObjectHandle storing the reference of
            /// invoked method. </param>
            /// <param name="methodBase">Methodbase being called in <c>Invoke</c>.</param>
            public Helper(ObjectHandle handle, MethodBase methodBase)
            {
                this.methodBase = methodBase;
                this.handle = handle;
            }

            /// <summary>
            /// Invokes the testcase
            /// </summary>
            public void Invoke()
            {
                this.methodBase.Invoke(this.handle.Unwrap(), null);
            }
        }
    }
}
