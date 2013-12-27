//-----------------------------------------------------------------------
// <copyright file="Helper.cs" company="Martin Brenn">
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
    using BurnSystems.Test;

    /// <summary>
    /// Type of container
    /// </summary>
    public enum ContainerType
    {
        /// <summary>
        /// Unknown container type
        /// </summary>
        Unknown = 0x00,

        /// <summary>
        /// Container contains type information
        /// </summary>
        Type = 0x01,

        /// <summary>
        /// Container contains data 
        /// </summary>
        Data = 0x02,

        /// <summary>
        /// Container contains the reference
        /// </summary>
        Reference = 0x03
    }

    /// <summary>
    /// Type of content of datacontainer
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// Unknown containertype
        /// </summary>
        Unknown = 0x00, 

        /// <summary>
        /// Datacontainer contains a <c>null</c>
        /// </summary>
        Null, 

        /// <summary>
        /// Datacontainer contains a native type
        /// </summary>
        Native,

        /// <summary>
        /// Datacontainer contains an array type
        /// </summary>
        Array,

        /// <summary>
        /// Datacontainer contains a complex type
        /// </summary>
        Complex,

        /// <summary>
        /// Enumeration object
        /// </summary>
        Enum
    }

    /// <summary>
    /// Implements some static helper methods, which are used for serialization
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Defines the streamheadertext, that will be written as header
        /// in the stream
        /// </summary>
        public const string StreamHeaderText = "BurnSystems.Serialization\n\n";

        /// <summary>
        /// Defines the used streamversion
        /// </summary>
        public static readonly Version StreamVersion = new Version(1, 0, 0, 0);

        /// <summary>
        /// Dictionary for converting number to native type
        /// </summary>
        private static Dictionary<int, Type> numberToNativeType = new Dictionary<int, Type>();

        /// <summary>
        /// Dictionary for converting type to number
        /// </summary>
        private static Dictionary<Type, int> nativeTypeToNumber = new Dictionary<Type, int>();

        /// <summary>
        /// Initializes static members of the Helper class.
        /// </summary>
        static Helper()
        {
            numberToNativeType[1] = typeof(bool);
            numberToNativeType[2] = typeof(byte);
            numberToNativeType[3] = typeof(short);
            numberToNativeType[4] = typeof(int);
            numberToNativeType[5] = typeof(long);
            numberToNativeType[6] = typeof(ushort);
            numberToNativeType[7] = typeof(ulong);
            numberToNativeType[8] = typeof(float);
            numberToNativeType[9] = typeof(double);
            numberToNativeType[10] = typeof(char);
            numberToNativeType[11] = typeof(string);

            foreach (var pair in numberToNativeType)
            {
                nativeTypeToNumber[pair.Value] = pair.Key;
            }
        }

        /// <summary>
        /// Checks, if the given type is a native type, which can be serialized directly
        /// </summary>
        /// <param name="type">Type to be cheked</param>
        /// <returns>true, if the given type is a native type</returns>
        public static bool IsNativeType(Type type)
        {
            Ensure.IsNotNull(type);

            return nativeTypeToNumber.ContainsKey(type);
        }

        /// <summary>
        /// Checks, if the given type is an enumeration
        /// </summary>
        /// <param name="type">Requested type</param>
        /// <returns>true, if requested type is enumeration</returns>
        public static bool IsEnumeration(Type type)
        {
            Ensure.IsNotNull(type);

            return type.IsEnum;
        }

        /// <summary>
        /// Chekcs, if the given type is an array type
        /// </summary>
        /// <param name="type">Type to be tested</param>
        /// <returns>true, if requested type is an arraytype</returns>
        public static bool IsArray(Type type)
        {
            return type.IsArray;
        }

        /// <summary>
        /// Converts the type to a number
        /// </summary>
        /// <param name="type">Type to be converted</param>
        /// <returns>Id of native type</returns>
        public static int ConvertNativeTypeToNumber(Type type)
        {
            return nativeTypeToNumber[type];
        }

        /// <summary>
        /// Converts a number to a native type
        /// </summary>
        /// <param name="typeId">Id of type</param>
        /// <returns>Type of the native type</returns>
        public static Type ConvertNumberToNativeType(int typeId)
        {
            return numberToNativeType[typeId];
        }
    }
}
