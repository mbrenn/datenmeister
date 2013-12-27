using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// Describes one item of a cached partition and additional data 
    /// used to manage this piece of data
    /// </summary>
    public class PartitionCacheItem
    {
        /// <summary>
        /// Gets or sets a value indicating whether the partition has been modified
        /// </summary>
        public bool IsModified
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cached partition
        /// </summary>
        public Partition Partition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the information when we had the last read access
        /// </summary>
        public DateTime LastReadAccess
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the information how many times this item had been accessed, while it was in
        /// cache. If item is removed from cache and reloaded, the number is 0. 
        /// </summary>
        public int AccessCountInCache
        {
            get;
            set;
        }
    }
}
