
//-----------------------------------------------------------------------
// <copyright file="PluginLoader.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using BurnSystems.Logging;

    /// <summary>
    /// This class implements a plugin loader for applications. 
    /// It also tries to resolve dependencies within plugins, so they
    /// are loaded in a correct order. 
    /// Plugins can be automatically loaded via AddLocalPluginsWithAttribute
    /// or explicitly added via AddPlugin
    /// </summary>
    public class PluginLoader<T> where T : class
    {
        /// <summary>
        /// Cache for loaded assemblies
        /// </summary>
        private Dictionary<string, Assembly> assemblies =
            new Dictionary<string, Assembly>();

        /// <summary>
        /// Stores the list of loaded plugins
        /// </summary>
        private List<PluginInfo<T>> plugins = new List<PluginInfo<T>>();

        /// <summary>
        /// Gets all plugins
        /// </summary>
        public List<PluginInfo<T>> Plugins
        {
            get { return this.plugins; }
        }

        /// <summary>
        /// Initializes a new instance of the PluginLoader class. 
        /// </summary>
        public PluginLoader()
        {
        }

        /// <summary>
        /// Loads all plugins from current directory with a certain attribute
        /// </summary>
        /// <param name="typeAttribute">Type of the attribute</param>
        public void AddLocalPluginsWithAttribute(Type typeAttribute)
        {
            foreach (var assemblyPath in Directory.GetFiles(Environment.CurrentDirectory, "*.dll"))
            {
                try
                {
                    var assembly = EnvironmentHelper.GetOrLoadAssembly(assemblyPath);

                    // Gets all types in assembly, which implement the interface IUmbraPlugin and 
                    // have the UmbraPluginAttribute
                    foreach (var type in assembly.GetTypes()
                        .Where(x => x.GetCustomAttributes(typeAttribute, false).Count() > 0)
                        .Where(x => x.GetInterfaces().Any(y => y.FullName == typeof(T).FullName)))
                    {
                        var plugin = Activator.CreateInstance(type) as T;
                        var pluginInfo = new PluginInfo<T>(assembly, type, plugin);

                        this.plugins.Add(pluginInfo);
                    }
                }
                catch (Exception exc)
                {
                    Log.TheLog.LogEntry(
                        LogEntry.Format(
                            LogLevel.Fail,
                            LocalizationBS.PluginLoader_AssemblyNotFound,
                            assemblyPath,
                            exc.Message));
                }
            }
        }

        /// <summary>
        /// Adds a certain plugin with a certain type within a certain assembly to the
        /// local database
        /// </summary>
        /// <param name="assemblyPath">Path tho assembly</param>
        /// <param name="typeName">Name of class, which should be loaded</param>
        public T AddPlugin(string assemblyPath, string typeName)
        {
            try
            {
                // Checks if assembly is in cache
                Assembly assembly;
                if (!this.assemblies.TryGetValue(assemblyPath, out assembly))
                {
                    assembly = EnvironmentHelper.GetOrLoadAssembly(assemblyPath);

                    this.assemblies[assemblyPath] = assembly;
                }

                // Gets type
                var typeOfPlugin = assembly.GetType(typeName);
                if (typeOfPlugin == null)
                {
                    throw new InvalidOperationException(
                        String.Format(
                            LocalizationBS.PluginLoader_TypeNotFound, typeName, assemblyPath));
                }

                if (!typeof(T).IsAssignableFrom(typeOfPlugin))
                {
                    throw new InvalidOperationException(
                        String.Format(
                            LocalizationBS.PluginLoader_NoPlugin, typeName));
                }

                // Creates the plugin and adds the plugin info
                var plugin = Activator.CreateInstance(typeOfPlugin) as T;
                var pluginInfo = new PluginInfo<T>(assembly, typeOfPlugin, plugin);

                this.plugins.Add(pluginInfo);

                return plugin;
            }
            catch (Exception exc)
            {
                var message = string.Format(
                    LocalizationBS.PluginLoader_FailedToLoadPlugin,
                    assemblyPath,
                    typeName);
                throw new InvalidOperationException(message, exc);
            }
        }

        /// <summary>
        /// Sorts all plugins according to their dependencies
        /// </summary>
        public void SortForDependencies()
        {
            // First sort the plugins
            // Source of order... After sorting, this class has to be empty
            var source = this.plugins.ToList();
            this.Plugins.Clear();

            bool hasAdded;
            do
            {
                hasAdded = false;
                for (var n = 0; n < source.Count; n++)
                {
                    var element = source[n];
                    if (element.Dependencies.All(
                        x => this.Plugins.Exists(y => y.Type == x)))
                    {
                        this.Plugins.Add(element);
                        source.RemoveAt(n);
                        n--;

                        hasAdded = true;
                    }
                }
            }
            while (hasAdded);

            if (source.Count != 0)
            {
                // Some plugins were not added... Dependency problem.
                foreach (var sourceEntry in source)
                {
                    var message = string.Format(
                        LocalizationBS.PluginLoader_InvalidDependency,
                        sourceEntry.Type.FullName);
                    Log.TheLog.LogEntry(
                        new LogEntry(message, LogLevel.Fatal));

                    foreach (var dependency in sourceEntry.Dependencies)
                    {
                        message = string.Format(
                            "    " + LocalizationBS.PluginLoader_Dependent,
                            dependency.FullName);
                        Log.TheLog.LogEntry(
                            new LogEntry(message, LogLevel.Fatal));
                    }
                }

                var types = source.Select(x => x.Type.FullName).Aggregate((x, y) => string.Format("{0}, {1}", x, y));
                throw new InvalidOperationException(types);
            }
        }
    }
}
