//-----------------------------------------------------------------------
// <copyright file="Composer.cs" company="Martin Brenn">
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
    using System.Runtime.Serialization;
    using BurnSystems.Collections;
    using BurnSystems.Logging;
    using BurnSystems.Test;

    /// <summary>
    /// The composer class helps to recompose the object
    /// </summary>
    public class Composer
    {
        /// <summary>
        /// These translations are used to translate the
        /// read values into the target type. 
        /// The dictionary stores in the key the pair of source and target-Type
        /// and in the value the transformation from source to targetobject
        /// </summary>
        private Dictionary<Pair<Type, Type>, Func<object, object>> translations =
            new Dictionary<Pair<Type, Type>, Func<object, object>>();

        /// <summary>
        /// Initializes a new instance of the Composer class.
        /// </summary>
        /// <param name="deserializer">Used deserializer</param>
        /// <param name="binaryReader">Used binaray reader</param>
        public Composer(Deserializer deserializer, BinaryReader binaryReader)
        {
            this.Deserializer = deserializer;
            this.BinaryReader = binaryReader;

            this.AddDefaultTranslations();
        }

        /// <summary>
        /// Gets or sets the deserializer
        /// </summary>
        public Deserializer Deserializer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the binary reader
        /// </summary>
        public BinaryReader BinaryReader
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a translation to the composer
        /// </summary>
        /// <typeparam name="TSource">Type of the source object</typeparam>
        /// <typeparam name="TTarget">Type of the target object</typeparam>
        /// <param name="translation">Translation function from source
        /// to target function</param>
        public void AddTranslation<TSource, TTarget>(Func<TSource, TTarget> translation)
        {
            this.translations[new Pair<Type, Type>(typeof(TSource), typeof(TTarget))]
                = x => translation((TSource)x);
        }

        /// <summary>
        /// Reads the object from the binary reader
        /// </summary>
        /// <returns>Read object</returns>
        public object ReadObject()
        {
            while (true)
            {
                var containerType = this.BinaryReader.ReadContainerType();

                switch (containerType)
                {
                    case ContainerType.Type:
                        this.ReadType();
                        break;
                    case ContainerType.Data:
                        return this.ReadData();
                    case ContainerType.Reference:
                        return this.ReadReference();
                    default:
                        throw new InvalidOperationException(
                            LocalizationBS.BinaryWriter_UnknownContainerType);
                }
            }
        }

        /// <summary>
        /// Reads a dataobject
        /// </summary>
        /// <returns>Read object</returns>
        public object ReadData()
        {
            var dataType = this.BinaryReader.ReadDataType();
            object result = null;

            switch (dataType)
            {
                case DataType.Null:
                    result = null;
                    break;
                case DataType.Native:
                    result = this.ReadNativeType();
                    break;
                case DataType.Array:
                    result = this.ReadArrayType();
                    break;
                case DataType.Complex:
                    result = this.ReadComplexType();
                    break;
                case DataType.Enum:
                    result = this.ReadEnumType();
                    break;
                default:
                    throw new InvalidOperationException(
                        LocalizationBS.BinaryWriter_UnknownDataType);
            }

            return result;
        }

        /// <summary>
        /// Reads a new datatype
        /// </summary>
        public void ReadType()
        {
            var typeEntry = this.BinaryReader.ReadTypeEntry();

            this.Deserializer.RegisterType(typeEntry);            
        }

        /// <summary>
        /// Reads the reference
        /// </summary>
        /// <returns>Read reference</returns>
        public object ReadReference()
        {
            var referenceHeader = this.BinaryReader.ReadReferenceHeader();

            return this.Deserializer.ObjectContainer.FindObjectById(referenceHeader.ObjectId);
        }

        /// <summary>
        /// Adds the default translations
        /// </summary>
        private void AddDefaultTranslations()
        {
            this.AddTranslation<long, int>(x => Convert.ToInt32(x));
            this.AddTranslation<int, long>(x => Convert.ToInt64(x));
            this.AddTranslation<float, double>(x => Convert.ToDouble(x));
            this.AddTranslation<double, float>(x => Convert.ToSingle(x));
        }

        /// <summary>
        /// Reads an enumeration
        /// </summary>
        /// <returns>Read enumeration</returns>
        private object ReadEnumType()
        {
            var typeId = this.BinaryReader.ReadInt64();
            var value = this.BinaryReader.ReadNativeObject();

            var typeEntry = this.Deserializer.TypeContainer.FindType(typeId);
            Ensure.IsNotNull(typeEntry);

            return Enum.ToObject(typeEntry.Type, value);
        }

        /// <summary>
        /// Reads and returns a complex type
        /// </summary>
        /// <returns>Read Complex type</returns>
        private object ReadComplexType()
        {
            // Reads complex header
            var complexHeader = this.BinaryReader.ReadComplexHeader();

            var type = this.Deserializer.TypeContainer.FindType(
                complexHeader.TypeId);
            Ensure.IsNotNull(type);

            var value = FormatterServices.GetSafeUninitializedObject(type.Type);
            this.Deserializer.ObjectContainer.AddObject(complexHeader.ObjectId, value);

            for (var n = 0; n < complexHeader.FieldCount; n++)
            {
                var propertyId = this.BinaryReader.ReadInt32();

                var valueProperty = this.ReadObject();

                // Tries to get field
                var field = type.FindField(propertyId);
                Ensure.IsNotNull(field);

                if (field.FieldInfo != null)
                {
                    if (valueProperty != null &&
                        !field.FieldInfo.FieldType.IsAssignableFrom(valueProperty.GetType()))
                    {
                        // Try to convert. 
                        var pair = new Pair<Type, Type>(
                            valueProperty.GetType(),
                            field.FieldInfo.FieldType);
                        Func<object, object> translator;
                        if (this.translations.TryGetValue(pair, out translator))
                        {
                            field.FieldInfo.SetValue(
                                value,
                                translator(valueProperty));

                            var logMessage = string.Format(
                                CultureInfo.InvariantCulture,
                                LocalizationBS.Composer_WrongTypeTransformed,
                                valueProperty.GetType().FullName,
                                field.FieldInfo.FieldType.FullName);

                            Log.TheLog.LogEntry(new LogEntry(
                                logMessage,
                                LogLevel.Message));
                        }
                        else
                        {
                            var logMessage = string.Format(
                                CultureInfo.InvariantCulture,
                                LocalizationBS.Composer_WrongTypeFound,
                                valueProperty.GetType().FullName,
                                field.FieldInfo.FieldType.FullName);

                            Log.TheLog.LogEntry(new LogEntry(
                                logMessage,
                                LogLevel.Fatal));
                        }
                    }
                    else
                    {
                        field.FieldInfo.SetValue(value, valueProperty);
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Reads and returns an array type
        /// </summary>
        /// <returns>Read Array Type</returns>
        private object ReadArrayType()
        {
            var arrayHeader = this.BinaryReader.ReadArrayHeader();

            // Gets the list
            var dimensions = arrayHeader.DimensionCount;
            var dimensionList = new List<int>();

            for (var n = 0; n < arrayHeader.DimensionCount; n++)
            {
                var length = arrayHeader.Dimensions[n];
                dimensionList.Add(length);
            }

            // Creates the array
            var elementType = this.Deserializer.TypeContainer.FindType(
                arrayHeader.TypeId);
            var array = Array.CreateInstance(elementType.Type, dimensionList.ToArray());
            this.Deserializer.ObjectContainer.AddObject(arrayHeader.ObjectId, array);

            // Enumerates the array
            int[] index = new int[dimensions];
            index.Initialize();

            var inloop = true;
            while (inloop)
            {
                // Check indizes
                for (var n = 0; n < dimensions; n++)
                {
                    if (index[n] >= dimensionList[n])
                    {
                        index[n] = 0;

                        if (n == dimensions - 1)
                        {
                            // Everything is finished
                            inloop = false;
                        }
                        else
                        {
                            // Increase next index
                            index[n + 1]++;
                        }
                    }
                }

                if (inloop)
                {
                    var value = this.ReadObject();
                    array.SetValue(value, index);

                    // Increase index
                    index[0]++;
                }
            }

            return array;
        }

        /// <summary>
        /// Reads native type
        /// </summary>
        /// <returns>Read native type</returns>
        private object ReadNativeType()
        {
            var value = this.BinaryReader.ReadNativeObject();
            return value;
        }
    }
}
