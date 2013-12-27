//-----------------------------------------------------------------------
// <copyright file="Serializer.cs" company="Martin Brenn">
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
    using System.IO;
    using BurnSystems.Test;

    /// <summary>
    /// Serializes an object into a stream
    /// </summary>
    [Obsolete("Do not use. Serializer is not aware of binary differences between x86 and x64")]
    public class Serializer : SerializationBase
    {
        /// <summary>
        /// Stream, where the object will be stored
        /// </summary>
        private Stream stream;

        /// <summary>
        /// The binarywriter to be used
        /// </summary>
        private BinaryWriter writer;

        /// <summary>
        /// Initializes a new instance of the Serializer class.
        /// </summary>
        /// <param name="stream">Stream to be used</param>
        public Serializer(Stream stream)
        {
            Ensure.IsNotNull(stream);
            this.stream = stream;
        }

        /// <summary>
        /// Serializes the object into the stream
        /// </summary>
        /// <param name="value">Value to be serialized</param>
        public void Serialize(object value)
        {            
            this.writer = new BinaryWriter(this.stream);
            var visitor = new Visitor(this, this.writer);

            // Writes header
            this.writer.WriteHeader();

            visitor.ParseObject(value);
        }

        /// <summary>
        /// Registers a type and returns the created entry. If this type is already registered, the
        /// registration data is returned without readding
        /// </summary>
        /// <param name="type">Type to be registered</param>
        /// <returns>Created entry</returns>
        public TypeEntry RegisterType(Type type)
        {
            var typeEntry = this.TypeContainer.FindType(type);

            if (typeEntry == null)
            {
                // Adds subgeneric
                if (type.IsGenericType)
                {
                    foreach (var genericType in type.GetGenericArguments())
                    {
                        this.RegisterType(genericType);
                    }
                }

                // Adds a new type
                typeEntry = this.TypeContainer.AddType(type);

                this.writer.StartContainer(ContainerType.Type);
                this.writer.WriteType(typeEntry);
            }

            return typeEntry;
        }
    }
}
