//-----------------------------------------------------------------------
// <copyright file="PluginInfo.cs" company="Martin Brenn">
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
    using System.Xml.Linq;
    using BurnSystems.Test;
    using System.Reflection;

    /// <summary>
    /// Stores the information about a plugin
    /// </summary>
    public class PluginInfo<T> where T : class
    {
        /// <summary>
        /// Stores a list of dependencies
        /// </summary>
        private List<Type> dependencies = new List<Type>();

        /// <summary>
        /// Gets or sets the instance of the plugin
        /// </summary>
        public T Instance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the assembly that has been loaded
        /// </summary>
        public Assembly Assembly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type that has been activated
        /// </summary>
        public Type Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a list of dependencies
        /// </summary>
        public List<Type> Dependencies
        {
            get { return this.dependencies; }
        }

        /// <summary>
        /// Initializes a new instance of the PluginInfo class.
        /// </summary>
        /// <param name="assembly">Assembly of the plugin</param>
        /// <param name="type">Type of the plugin</param>
        /// <param name="instance">Instance of the object that has been created</param>
        public PluginInfo(Assembly assembly, Type type, T instance)
        {
            this.Assembly = assembly;
            this.Type = type;
            this.Instance = instance;
            this.UpdateDependencies();
        }

        /// <summary>
        /// Updates the dependencies of the plugin
        /// </summary>
        private void UpdateDependencies()
        {
            this.Dependencies.Clear();

            var attributes = this.Type.GetCustomAttributes(
                typeof(PluginDependencyAttribute), true);
            foreach (var attributeRaw in attributes)
            {
                var attribute = attributeRaw as PluginDependencyAttribute;
                Ensure.IsNotNull(attribute);

                this.Dependencies.Add(attribute.Type);
            }
        }

        /// <summary>
        /// Converts the plugininfo to a string
        /// </summary>
        /// <returns>Name of the associated type</returns>
        public override string ToString()
        {
            if (this.Type != null)
            {
                return this.Type.FullName;
            }

            return "PluginInfo, Unknown Type";
        }
    }
}
