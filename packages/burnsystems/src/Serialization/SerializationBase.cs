//-----------------------------------------------------------------------
// <copyright file="SerializationBase.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The base class for serialization and deserialization.
    /// </summary>
    public class SerializationBase
    {
        /// <summary>
        /// Container, storing the objects
        /// </summary>
        private ObjectContainer objectContainer = new ObjectContainer();

        /// <summary>
        /// Container, storing the types
        /// </summary>
        private TypeContainer typeContainer = new TypeContainer();

        /// <summary>
        /// Gets the typecontainer
        /// </summary>
        public TypeContainer TypeContainer
        {
            get { return this.typeContainer; }
        }

        /// <summary>
        /// Gets the container, storing the object
        /// </summary>
        internal ObjectContainer ObjectContainer
        {
            get { return this.objectContainer; }
        }

        /// <summary>
        /// Registers a new object
        /// </summary>
        /// <param name="value">Value to be registered</param>
        /// <param name="alreadyInserted">Value, indicating whether this object has alredy been inserted.</param>
        /// <returns>Id of registered object</returns>
        public long RegisterObject(object value, out bool alreadyInserted)
        {
            return this.objectContainer.AddObject(value, out alreadyInserted);
        }
    }
}
