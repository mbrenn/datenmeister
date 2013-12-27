//-----------------------------------------------------------------------
// <copyright file="FlexibleVariable.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Parser.Helper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using BurnSystems.Interfaces;

    /// <summary>
    /// This class offers a flexible variable, which is a parser object
    /// and can contain properties and subitems. 
    /// This class can be used, if no special classes should be created
    /// for a specific task.
    /// </summary>
    public class FlexibleVariable : IParserObject, IEnumerable, IEnumerable<object>
    {
        /// <summary>
        /// Subitems of the variable
        /// </summary>
        private List<object> subItems =
            new List<object>();

        /// <summary>
        /// Properties of variable
        /// </summary>
        private Dictionary<string, object> properties =
            new Dictionary<string, object>();
                
        /// <summary>
        /// Gets or sets a property
        /// </summary>
        /// <param name="key">Key of property</param>
        /// <returns>Value of property</returns>
        public object this[string key]
        {
            get
            {
                object result = null;
                if (this.properties.TryGetValue(key, out result))
                {
                    return result;
                }

                // Creates a new flexible variable for unknown properties
                FlexibleVariable variable = new FlexibleVariable();
                this.properties[key] = variable;
                return variable;
            }

            set
            {
                this.properties[key] = value;
            }
        }

        /// <summary>
        /// Fügt ein neues Objekt hinzu
        /// </summary>
        /// <param name="item">Hinzuzufügendes Objekt</param>
        public void AddItem(object item)
        {
            this.subItems.Add(item);
        }

        /// <summary>
        /// Gets a property
        /// </summary>
        /// <param name="name">Name of property</param>
        /// <returns>Value of property</returns>
        public object GetProperty(string name)
        {
            object result;
            if (this.properties.TryGetValue(name, out result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Führt eine benutzerdefinierte Funktion aus
        /// </summary>
        /// <param name="functionName">Name der function</param>
        /// <param name="parameters">Parameter of function</param>
        /// <returns>null, da dieses Objekt keine Funktion implementiert</returns>
        public object ExecuteFunction(string functionName, IList<object> parameters)
        {
            return null;
        }

        /// <summary>
        /// Gibt die Aufzählung für die Subitems zurück
        /// </summary>
        /// <returns>The Enumerator of subitems</returns>
        public IEnumerator GetEnumerator()
        {
            return this.subItems.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator 
        /// </summary>
        /// <returns>Enumerator for all subitems</returns>
        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            foreach (object item in this.subItems)
            {
                yield return item;
            }
        }
    }
}
