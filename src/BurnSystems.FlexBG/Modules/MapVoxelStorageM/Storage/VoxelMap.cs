using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// Defines the voxelmap class, which gives the access to voxels for the game
    /// </summary>
    public class VoxelMap : IVoxelMap
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(VoxelMap));

        /// <summary>
        /// Defines the loader for the partitions
        /// </summary>
        [Inject]
        public IPartitionLoader Loader
        {
            get;
            set;
        }

        public VoxelMapInfo GetInfo(long instanceId)
        {
            return this.Loader.LoadInfoData(instanceId);
        }

        /// <summary>
        /// Gets the info, if map has been created
        /// </summary>
        public bool IsMapCreated(long instanceId)
        {
            return this.Loader.LoadInfoData(instanceId) != null;
        }

        /// <summary>
        /// Initializes a new instance of the voxelmap
        /// </summary>
        /// <param name="info">Information about voxelmap</param>
        public VoxelMap()
        {
        }

        /// <summary>
        /// Syncronisation for locking
        /// </summary>
        private ReaderWriterLockSlim sync = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// Calculates the number of partition and the relative coordinates for a certain field
        /// </summary>
        /// <param name="columnX">X-Coordinate of queried column</param>
        /// <param name="columnY">Y-Coordinate of queried column</param>
        /// <param name="partitionX">X Coordinate of the partition itself</param>
        /// <param name="partitionY">Y Coordinate of the partition itself</param>
        /// <param name="relX">Relative X coordinate of the column on the partition</param>
        /// <param name="relY">Relative Y coordinate of the column on the partition</param>
        public void CalculatePartitionCoordinates(long instanceId, int columnX, int columnY, out int partitionX, out int partitionY, out int relX, out int relY)
        {
            var info = this.Loader.LoadInfoData(instanceId);
            Ensure.That(info != null, "No Info Data for instance " + instanceId);

            Ensure.That(columnX >= 0 && columnY >= 0
                && columnX < info.SizeX && columnY < info.SizeY);

            partitionX = columnX / info.PartitionLength;
            partitionY = columnY / info.PartitionLength;
            relX = columnX % info.PartitionLength;
            relY = columnY % info.PartitionLength;
        }


        /// <summary>
        /// Calculates the absolute field position from relative coordinates on a partition
        /// </summary>
        /// <param name="partitionX">X Coordinate of the partition itself</param>
        /// <param name="partitionY">Y Coordinate of the partition itself</param>
        /// <param name="relX">Relative X coordinate of the column on the partition</param>
        /// <param name="relY">Relative Y coordinate of the column on the partition</param>
        /// <param name="columnX">X-Coordinate of queried column</param>
        /// <param name="columnY">Y-Coordinate of queried column</param>
        public void CalculateFieldCoordinates(long instanceId, int partitionX, int partitionY, int relX, int relY, out int columnX, out int columnY)
        {
            var info = this.Loader.LoadInfoData(instanceId);

            columnX = info.PartitionLength * partitionX + relX;
            columnY = info.PartitionLength * partitionY + relY;
        }

        /// <summary>
        /// Creates a simple mal just containing air
        /// </summary>
        public void CreateMap(long instanceId, VoxelMapInfo info)
        {
            // Creates the map by creating partition and storing them
            var partitionCountX = info.SizeX / info.PartitionLength;
            var partitionCountY = info.SizeY / info.PartitionLength;
            info.InstanceId = instanceId;

            this.Loader.StoreInfoData(instanceId, info);

            for (var x = 0; x < partitionCountX; x++)
            {
                for (var y = 0; y < partitionCountY; y++)
                {
                    var partition = this.Loader.LoadPartition(instanceId, x, y);
                    partition.InitFields();
                    this.Loader.StorePartition(partition);
                }
            }
        }

        /// <summary>
        /// Sets the field type for a certain range
        /// </summary>
        /// <param name="column">Column of the fieldtype</param>
        /// <param name="fieldType">Fieldtype to be set</param>
        /// <param name="startingHeight">Starting Height of new field type</param>
        /// <param name="endingHeight">Ending height of new field type</param>
        public void SetFieldType(long instanceId, int x, int y, byte fieldType, float startingHeight, float endingHeight)
        {
            // 1: Convert partition
            int relX, relY;
            var partition = this.GetPartitionFor(instanceId, x, y, out relX, out relY);

            // 3. Change partition
            partition.SetFieldType(relX, relY, fieldType, startingHeight, endingHeight);

            // 4. Store partition
            this.Loader.StorePartition(partition);
        }

        /// <summary>
        /// Sets the field type for a certain range
        /// </summary>
        /// <param name="column">Column of the fieldtype</param>
        /// <param name="fieldType">Fieldtype to be set</param>
        /// <param name="startingHeight">Starting Height of new field type</param>
        /// <param name="endingHeight">Ending height of new field type</param>
        public byte GetFieldType(long instanceId, int x, int y, float height)
        {
            int relX;
            int relY;
            var partition = GetPartitionFor(instanceId, x, y, out relX, out relY);

            // 3. Change partition
            return partition.GetFieldType(relX, relY, height);
        }

        /// <summary>
        /// Sets the data for one column
        /// </summary>
        /// <param name="instanceId">Id of the instance whose column shall be modified</param>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        /// <param name="key">Key of the data</param>
        /// <param name="data">Data to be assinged</param>
        public void SetData(long instanceId, int x, int y, int key, byte[] data)
        {
            // 1: Convert partition
            int relX, relY;
            var partition = this.GetPartitionFor(instanceId, x, y, out relX, out relY);

            // 3. Change partition
            var column = partition.GetColumn(relX, relY);
            column.Set(key, data);

            // 4. Store partition
            this.Loader.StorePartition(partition);
        }

        /// <summary>
        /// Gets the data for one column
        /// </summary>
        /// <param name="instanceId">Id of the instance whose column shall be read-out</param>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        /// <param name="key">Key of the data</param>
        public byte[] GetData(long instanceId, int x, int y, int key)
        {
            int relX;
            int relY;
            var partition = GetPartitionFor(instanceId, x, y, out relX, out relY);
            return partition.GetColumn(relX, relY).Get(key);
        }

        /// <summary>
        /// Gets the surface information
        /// </summary>
        /// <param name="x1">Upper left X-Coordinate</param>
        /// <param name="y1">Upper left Y-Coordinate</param>
        /// <param name="x2">Lower right X-Coordinate</param>
        /// <param name="y2">Lower right Y-Coordinate</param>
        /// <returns>Information about the fieldtypes. This array is starting with 0 relative to x1/y1.First coordinate is x-coordinate</returns>
        public FieldTypeChangeInfo[][] GetSurfaceInfo(long instanceId, int x1, int y1, int x2, int y2)
        {
            Ensure.That(x1 <= x2);
            Ensure.That(y1 <= y2);

            var result = new FieldTypeChangeInfo[x2 - x1 + 1][];

            for (var x = 0; x <= x2 - x1; x++)
            {
                result[x] = new FieldTypeChangeInfo[y2 - y1 + 1];
            }

            for (var x = x1; x <= x2; x++)
            {
                for (var y = y1; y <= y2; y++)
                {
                    int partitionX, partitionY, relX, relY;
                    this.CalculatePartitionCoordinates(instanceId, x, y, out partitionX, out partitionY, out relX, out relY);

                    var partition = this.Loader.LoadPartition(instanceId, partitionX, partitionY);
                    var column = partition.GetColumn(relX, relY).Changes;

                    if (column.Count == 1)
                    {
                        result[x - x1][y - y1].ChangeHeight = float.MinValue;
                        result[x - x1][y - y1].FieldType = 0;
                    }
                    else
                    {
                        result[x - x1][y - y1].ChangeHeight = column[1].ChangeHeight;
                        result[x - x1][y - y1].FieldType = column[1].FieldType;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a complete column of a certain position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public VoxelMapColumn GetColumn(long instanceId, int x, int y)
        {
            int relX, relY;
            var partition = GetPartitionFor(instanceId, x, y, out relX, out relY);

            // 3. Change partition
            return partition.GetColumn(relX, relY);
        }

        /// <summary>
        /// Gets a complete column of a certain position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public void SetColumn(long instanceId, int x, int y, VoxelMapColumn column)
        {
            int relX, relY;
            var partition = GetPartitionFor(instanceId, x, y, out relX, out relY);

            // 3. Change partition
            partition.SetColumn(relX, relY, column);
        }

        /// <summary>
        /// Gets a partition for a certain position
        /// </summary>
        /// <param name="x">Absolute X-Coordinate</param>
        /// <param name="y">Absolute Y-Coordinate</param>
        /// <param name="relX">Relative X-Coordinate</param>
        /// <param name="relY">Relative Y-Coordinate</param>
        /// <returns>Loaded partition</returns>
        private Partition GetPartitionFor(long instanceId, int x, int y, out int relX, out int relY)
        {
            // 1: Convert partition
            int partitionX;
            int partitionY;

            this.CalculatePartitionCoordinates(instanceId, x, y, out partitionX, out partitionY, out relX, out relY);

            // 2. Get Partition
            var partition = this.Loader.LoadPartition(instanceId, partitionX, partitionY);
            Ensure.That(partition != null);

            return partition;
        }

        /// <summary>
        /// Acquires readlock for a certain column
        /// </summary>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        public void AcquireReadLock(long instanceId, int x1, int y1, int x2, int y2)
        {
            sync.EnterReadLock();
        }

        /// <summary>
        /// Acquires writelock for a certain column
        /// </summary>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        public void AcquireWriteLock(long instanceId, int x1, int y1, int x2, int y2)
        {
            sync.EnterWriteLock();
        }

        /// <summary>
        /// Releases readlock for a certain column
        /// </summary>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        public void ReleaseReadLock(long instanceId, int x1, int y1, int x2, int y2)
        {
            sync.ExitReadLock();
        }

        /// <summary>
        /// Releases writelock for a certain column
        /// </summary>
        /// <param name="x">X-Coordinate of the column</param>
        /// <param name="y">Y-Coordinate of the column</param>
        public void ReleaseWriteLock(long instanceId, int x1, int y1, int x2, int y2)
        {
            sync.ExitWriteLock();
        }
    }
}
