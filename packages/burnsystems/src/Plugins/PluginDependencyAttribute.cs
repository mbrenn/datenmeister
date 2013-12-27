//-----------------------------------------------------------------------
// <copyright file="PluginDependencyAttribute.cs" company="Martin Brenn">
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

    /// <summary>
    /// This attribute is used to define the dependency of a plugin 
    /// to another plugin. The Pluginloader has to load the plugins in 
    /// the correct order. 
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class PluginDependencyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the PluginDependencyAttribute class.
        /// </summary>
        /// <param name="type">Parameter to which the type is dependend</param>
        public PluginDependencyAttribute(Type type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Gets the type to which this plugin is dependend
        /// </summary>
        public Type Type
        {
            get;
            internal set;
        }
    }
}
