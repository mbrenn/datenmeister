//-----------------------------------------------------------------------
// <copyright file="BinaryWriter.cs" company="Martin Brenn">
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
    using System.Globalization;
    using System.IO;
    using System.Text;

    /// <summary>
    /// This class is an implementation of a binary writer for serialization.
    /// </summary>
    public class BinaryWriter
    {
        /// <summary>
        /// Stream to be used for serialization
        /// </summary>
        private Stream stream;

        /// <summary>
        /// Initializes a new instance of the BinaryWriter class.
        /// </summary>
        /// <param name="stream">Stream to be used for serialization</param>
        public BinaryWriter(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// Converts the native object to a byte-array
        /// </summary>
        /// <param name="value">Object to be converted</param>
        /// <param name="type">Type of object</param>
        /// <returns>Bytearray with the converted value</returns>
        public static byte[] ConvertNativeObject(object value, Type type)
        {
            if (type == typeof(bool))
            {
                return new byte[] { (byte)(((bool)value) ? 0x01 : 0x00) };
            }

            if (type == typeof(byte))
            {
                return new byte[] { (byte)value };
            }

            if (type == typeof(short))
            {
                return BitConverter.GetBytes((short)value);
            }

            if (type == typeof(int))
            {
                return BitConverter.GetBytes((int)value);
            }

            if (type == typeof(long))
            {
                return BitConverter.GetBytes((long)value);
            }

            if (type == typeof(ushort))
            {
                return BitConverter.GetBytes((ushort)value);
            }

            if (type == typeof(uint))
            {
                return BitConverter.GetBytes((uint)value);
            }

            if (type == typeof(ulong))
            {
                return BitConverter.GetBytes((ulong)value);
            }

            if (type == typeof(float))
            {
                return BitConverter.GetBytes((float)value);
            }

            if (type == typeof(double))
            {
                return BitConverter.GetBytes((double)value);
            }

            if (type == typeof(char))
            {
                return BitConverter.GetBytes((char)value);
            }

            if (type == typeof(string))
            {
                return Encoding.UTF8.GetBytes((string)value);
            }

            throw new InvalidOperationException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    LocalizationBS.BinaryWriter_ObjectNotConverted,
                    value.GetType()));
        }        

        /// <summary>
        /// Writes the header into the stream
        /// </summary>
        public void WriteHeader()
        {
            var bytes = Encoding.UTF8.GetBytes(Helper.StreamHeaderText);
            this.stream.Write(bytes, 0, bytes.Length);

            byte[] versionBytes = new[]
            {
                (byte)Helper.StreamVersion.Major,
                (byte)Helper.StreamVersion.Minor,
                (byte)Helper.StreamVersion.Build,
                (byte)Helper.StreamVersion.Revision
            };

            this.stream.Write(versionBytes, 0, versionBytes.Length);
        }

        /// <summary>
        /// Starts to write a container
        /// </summary>
        /// <param name="containerType">Containertype to be started</param>
        public void StartContainer(ContainerType containerType)
        {
            switch (containerType)
            {
                case ContainerType.Type:
                    this.stream.WriteByte(0x01);
                    break;
                case ContainerType.Data:
                    this.stream.WriteByte(0x02);
                    break;
                case ContainerType.Reference:
                    this.stream.WriteByte(0x03);
                    break;
                default:
                    throw new InvalidOperationException(LocalizationBS.BinaryWriter_UnknownContainerType);
            }            
        }

        /// <summary>
        /// Writes a type to stream
        /// </summary>
        /// <param name="typeEntry">Typeentry to be written</param>
        public void WriteType(TypeEntry typeEntry)
        {
            // Writes Type if
            this.WriteInt64(typeEntry.TypeId);

            // Writes typename
            var typeNameAsBytes = Encoding.UTF8.GetBytes(typeEntry.Name);

            this.WriteInt32(typeNameAsBytes.Length);
            this.stream.Write(typeNameAsBytes, 0, typeNameAsBytes.Length);

            this.WriteInt32(typeEntry.GenericArguments.Count);
            foreach (var typeEntryId in typeEntry.GenericArguments)            
            {
                this.WriteInt64(typeEntryId);
            }

            // Writes number of fields
            this.WriteInt32(typeEntry.Fields.Count);

            // Write fields
            foreach (var field in typeEntry.Fields)
            {
                // Field id
                this.WriteInt32(field.FieldId);

                // Fieldname
                var propertyNameAsBytes = Encoding.UTF8.GetBytes(field.Name);

                this.WriteInt32(propertyNameAsBytes.Length);
                this.stream.Write(propertyNameAsBytes, 0, propertyNameAsBytes.Length);
            }
        }

        /// <summary>
        /// Starts the datacontainer
        /// </summary>
        /// <param name="dataType">Type of datacontainer to be started</param>
        public void StartDataContainer(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Null:
                    this.stream.WriteByte(0x01);
                    break;
                case DataType.Native:
                    this.stream.WriteByte(0x02);
                    break;
                case DataType.Array:
                    this.stream.WriteByte(0x03);
                    break;
                case DataType.Complex:
                    this.stream.WriteByte(0x04);
                    break;
                case DataType.Enum:
                    this.stream.WriteByte(0x05);
                    break;
                default:
                    throw new InvalidOperationException(LocalizationBS.BinaryWriter_UnknownContainerType);
            }            
        }

        /// <summary>
        /// Writes a native type
        /// </summary>
        /// <param name="value">Object to be written</param>
        public void WriteNativeType(object value)
        {
            var type = value.GetType();
            var nativeTypeId = Helper.ConvertNativeTypeToNumber(type);

            // Write native type
            this.WriteInt32(nativeTypeId);

            // Gets the value
            var valueBytes = ConvertNativeObject(value, type);

            // Writes length and value
            this.WriteInt32(valueBytes.Length);
            this.stream.Write(valueBytes, 0, valueBytes.Length);
        }

        /// <summary>
        /// Writes a reference container
        /// </summary>
        /// <param name="objectId">Id of reference object</param>
        public void WriteReference(long objectId)
        {
            this.WriteInt64(objectId);
        }

        /// <summary>
        /// Starts the complextype
        /// </summary>
        /// <param name="typeId">Id of type</param>
        /// <param name="objectId">Id of object</param>
        /// <param name="fieldCount">Number of fields</param>
        public void StartComplexType(long typeId, long objectId, int fieldCount)
        {
            this.WriteInt64(typeId);

            this.WriteInt64(objectId);

            this.WriteInt32(fieldCount);
        }

        /// <summary>
        /// Starts the array type 
        /// </summary>
        /// <param name="elementTypeId">Type of the underlying element type of array</param>
        /// <param name="objectId">Id of the object</param>
        /// <param name="dimensionCount">Number of dimensions</param>
        /// <param name="dimensions">Array of Dimensions</param>
        public void StartArrayType(long elementTypeId, long objectId, int dimensionCount, IEnumerable<int> dimensions)
        {
            this.WriteInt64(elementTypeId);

            this.WriteInt64(objectId);

            this.WriteInt32(dimensionCount);

            foreach (var dimension in dimensions)
            {
                this.WriteInt32(dimension);
            }
        }

        /// <summary>
        /// Writes the property id
        /// </summary>
        /// <param name="propertyId">Id of property to be added</param>
        public void WritePropertyId(int propertyId)
        {
            this.WriteInt32(propertyId);
        }

        /// <summary>
        /// Writes a 32 bit integer value
        /// </summary>
        /// <param name="value">Value to be written</param>
        public void WriteInt32(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            this.stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes a 64 bit integer value
        /// </summary>
        /// <param name="value">Value to be written</param>
        public void WriteInt64(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            this.stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes an enumeration object
        /// </summary>
        /// <param name="typeId">Id of enumeration</param>
        /// <param name="enumeration">Value of enumeration</param>
        internal void WriteEnumType(long typeId, Enum enumeration)
        {
            this.WriteInt64(typeId);

            this.WriteNativeType((enumeration as IConvertible).ToInt32(null));
        }
    }
}
