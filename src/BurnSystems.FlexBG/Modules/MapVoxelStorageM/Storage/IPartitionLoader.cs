using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    public interface IPartitionLoader
    {
        /// <summary>
        /// Loads a partition from drive
        /// </summary>
        /// <param name="instanceId">Id of the instance of map</param>
        /// <param name="x">X-Coordinate of the partition</param>
        /// <param name="y">Y-Coordinate of the partition</param>
        /// <returns>Loaded partition</returns>
        Partition LoadPartition(long instanceId, int x, int y);

        /// <summary>
        /// Stores a certain partition on drive
        /// </summary>
        /// <param name="partition">Partition to be stored</param>
        void StorePartition(Partition partition);

        /// <summary>
        /// Stores the info data
        /// </summary>
        void StoreInfoData(long instanceId, VoxelMapInfo info);

        /// <summary>
        /// Loads the info data from generic file
        /// </summary>
        /// <returns></returns>
        VoxelMapInfo LoadInfoData(long instanceId);
    }
}
