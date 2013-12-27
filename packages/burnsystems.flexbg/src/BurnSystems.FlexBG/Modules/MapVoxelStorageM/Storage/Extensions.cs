using BurnSystems.Logging;
using BurnSystems.Test;
using System;
using System.Collections.Generic;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// Stores some extension methods make access easy
    /// </summary>
    public static class Extensions
    {
        static ILog logger = new ClassLogger(typeof(Extensions));

        /// <summary>
        /// Initializes the columns
        /// </summary>
        /// <param name="list"></param>
        public static void InitFields(this VoxelMapColumn list)
        {
            // First: Clear
            list.Changes.Clear();

            // Add air
            var info = new FieldTypeChangeInfo();
            info.Init();

            list.Changes.Add(info);
        }

        /// <summary>
        /// Checks, if the column is valud
        /// </summary>
        /// <param name="column">Column to be checked</param>
        /// <returns>true, if column is valid</returns>
        public static bool IsValid(this VoxelMapColumn column)
        {
            // Checks, if we have a value
            if (column.Changes.Count <= 0)
            {
                logger.LogEntry(LogLevel.Fail, "Column is not valid: column.Count <= 0");
                return false;
            }

            // Checks, if we have a maximum value
            if (column.Changes[0].ChangeHeight != float.MaxValue)
            {
                logger.LogEntry(LogLevel.Fail, "Column is not valid: column[0].ChangeHeight != float.MaxValue");
                return false;
            }

            // Checks if height information is ordered
            var lastHeight = float.MaxValue;
            var lastFieldType = column.Changes[0].FieldType;
            for (var m = 1; m < column.Changes.Count; m++)
            {
                var newHeight = column.Changes[m].ChangeHeight;
                var newFieldType = column.Changes[m].FieldType;
                if (newHeight >= lastHeight)
                {
                    logger.LogEntry(LogLevel.Fail, "Column is not valid: Not correctly ordered");
                    return false;
                }

                if (newFieldType == lastFieldType)
                {
                    // Field types have to change, otherwise we do not have a field change
                    logger.LogEntry(LogLevel.Fail, "Column is not valid: Fieldtype has not changed");
                    return false;
                }

                lastHeight = newHeight;
                lastFieldType = newFieldType;
            }

            // Checks, if two data items have the same key. They shall not have the same key
            var hasFound = new List<int>();
            foreach (var data in column.Data)
            {
                if (hasFound.Contains(data.Key))
                {
                    logger.LogEntry(LogLevel.Fail, "Column is not valid: Two data items with key: " + data.Key.ToString());
                    return false;
                }

                hasFound.Add(data.Key);
            }

            return true;
        }

        /// <summary>
        /// Gets the field type for a certain column at a certain height
        /// </summary>
        /// <param name="column">Column to be queried</param>
        /// <param name="height">Height of the map</param>
        /// <returns>Fieldtype</returns>
        public static byte GetFieldType(this VoxelMapColumn column, float height)
        {
            byte result = 0;

            foreach (var fieldTypeInfo in column.Changes)
            {
                if (fieldTypeInfo.ChangeHeight >= height)
                {
                    result = fieldTypeInfo.FieldType;
                }
                else
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the field type for a certain range
        /// </summary>
        /// <param name="column">Column of the fieldtype</param>
        /// <param name="fieldType">Fieldtype to be set</param>
        /// <param name="startingHeight">Starting Height of new field type</param>
        /// <param name="endingHeight">Ending height of new field type</param>
        public static void SetFieldType(this VoxelMapColumn column, byte fieldType, float startingHeight, float endingHeight)
        {
            if (startingHeight < endingHeight)
            {
                SetFieldType(column, fieldType, endingHeight, startingHeight);
                return;
            }

            if (startingHeight == endingHeight)
            {
                // Nothing to do
                return;
            }

            // Findout now the current fieldtype at start and end
            var startingFieldType = column.GetFieldType(startingHeight);
            var endingFieldType = column.GetFieldType(endingHeight);

            // Removes all fields equal to or between boundaries
            column.Changes.RemoveAll(x =>
                {
                    var height = x.ChangeHeight;
                    return height <= startingHeight && height >= endingHeight;
                });

            if (column.GetFieldType(startingHeight) != fieldType || column.Changes.Count == 0)
            {
                // Adds top with new fieldtype
                var topField = new FieldTypeChangeInfo();
                topField.FieldType = fieldType;
                topField.ChangeHeight = startingHeight;
                column.Changes.Add(topField);
            }

            // Sorts
            column.Changes.Sort((x, y) =>
            {
                return y.ChangeHeight.CompareTo(x.ChangeHeight);
            });

            if (column.GetFieldType(endingHeight) != endingFieldType && endingHeight > float.MinValue)
            {
                // Adds bottom changing back to old field type
                var endField = new FieldTypeChangeInfo();
                endField.FieldType = endingFieldType;
                endField.ChangeHeight = endingHeight;
                column.Changes.Add(endField);
            }

            // Sorts
            column.Changes.Sort((x, y) =>
                {
                    return y.ChangeHeight.CompareTo(x.ChangeHeight);
                });

            // Check, if everything is correct
#if DEBUG
            Ensure.That(column.IsValid());
#endif
        }


        /// <summary>
        /// Gets an enumeration of heights, where a change to this fieldtype occurs. 
        /// If no change occures, null is returned
        /// </summary>
        /// <param name="column">Column to be evaluated</param>
        /// <param name="fieldType">Fieldtype being queried</param>
        public static IEnumerable<float> GetHeightsOfFieldType(this VoxelMapColumn column, byte fieldType)
        {
            for(var n = 0; n < column.Changes.Count; n++)
            {
                var change = column.Changes[n];
                if (change.FieldType == fieldType)
                {
                    yield return change.ChangeHeight;
                }
            }
        }

        /// <summary>
        /// Gets the height for the given column
        /// </summary>
        /// <param name="column">Column to be used</param>
        /// <returns>Height of first field change</returns>
        public static double GetHeight(this VoxelMapColumn column)
        {
            if (column == null || column.Changes.Count <= 1)
            {
                // Column has not been found, maximum height
                // Or column just consists of air
                return double.MaxValue;
            }

            return column.Changes[1].ChangeHeight;
        }

        /// <summary>
        /// Gets the height for the given column
        /// </summary>
        /// <param name="column">Column to be used</param>
        /// <returns>Height of first field change</returns>
        public static double GetHeight(this IVoxelMap voxelMap, long instanceId, int x, int y)
        {
            var column = voxelMap.GetColumn(instanceId, x, y);
            if (column == null)
            {
                return double.MaxValue;
            }

            return column.GetHeight();
        }

        public static void SetDataByInt64(this IVoxelMap voxelMap, long instanceId, int x, int y, int dataKey, long data)
        {
            voxelMap.SetData(
                instanceId,
                x,
                y,
                dataKey,
                BitConverter.GetBytes(data));
        }

        public static int GetDataAsInt32(this IVoxelMap voxelMap, long instanceId, int x, int y, int dataKey)
        {
            var bytes = voxelMap.GetData(
                instanceId,
                x,
                y,
                dataKey);

            if (bytes == null)
            {
                return 0;
            }
            else
            {
                Ensure.That(bytes.Length == 4, "Data is not 4 bytes long");
                return BitConverter.ToInt32(bytes, 0);
            }
        }

        public static long GetDataAsInt64(this IVoxelMap voxelMap, long instanceId, int x, int y, int dataKey)
        {
            var bytes = voxelMap.GetData(
                instanceId,
                x,
                y,
                dataKey);

            if (bytes == null)
            {
                return 0;
            }
            else
            {
                Ensure.That(bytes.Length == 8, "Data is not 8 bytes long");
                return BitConverter.ToInt64(bytes,0);
            }
        }
    }
}
