using BurnSystems.FlexBG.Interfaces;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage
{
    /// <summary>
    /// Performs the caching of partitions.
    /// The methods are also thread-safe
    /// </summary>
    [BindAlsoTo(typeof(IFlexBgRuntimeModule))]
    public class PartitionCache : IPartitionLoader, IFlexBgRuntimeModule
    {
        private ILog logger = new ClassLogger(typeof(PartitionCache));

        /// <summary>
        /// Defines the lock
        /// </summary>
        private ReaderWriterLockSlim sync = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// Stores the info cache
        /// </summary>
        private Dictionary<long, VoxelMapInfo> infoCache = new Dictionary<long, VoxelMapInfo>();

        /// <summary>
        /// Stores a list of cached partitions
        /// </summary>
        private PartitionCacheItem[] cachedPartitions;

        /// <summary>
        /// Gets or sets the database instance
        /// </summary>
        private IPartitionLoader Database
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the number of allowed partitions in cache
        /// </summary>
        public int CacheSize
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of Database cache
        /// </summary>
        /// <param name="database"></param>
        public PartitionCache(IPartitionLoader database)
            : this(database, 30)
        {
        }

        /// <summary>
        /// Initializes a new instance of Database cache
        /// </summary>
        /// <param name="database"></param>
        /// <param name="cacheSize">Defines the cachesize</param>
        public PartitionCache(IPartitionLoader database, int cacheSize)
        {
            this.Database = database;
            this.CacheSize = cacheSize;
            this.cachedPartitions = new PartitionCacheItem[this.CacheSize];
        }

        /// <summary>
        /// Loads a partition from drive
        /// </summary>
        /// <param name="x">X-Coordinate of the partition</param>
        /// <param name="y">Y-Coordinate of the partition</param>
        /// <returns>Loaded partition</returns>
        public Partition LoadPartition(long instanceId, int x, int y)
        {
            var cachedItem = this.FindOrLoadPartition(instanceId, x, y);
            return cachedItem.Partition;
        }

        /// <summary>
        /// Finds a partition in cache or loads one partition and stores it in cache
        /// </summary>
        /// <param name="x">X-Coordinate of the partition (not real)</param>
        /// <param name="y">Y-Coordinate of the partition (not real)</param>
        private PartitionCacheItem FindOrLoadPartition(long instanceId, int x, int y)
        {
            try
            {
                this.sync.EnterUpgradeableReadLock();

                for (var n = 0; n < this.CacheSize; n++)
                {
                    var item = this.cachedPartitions[n];

                    if (item != null &&
                        item.Partition.InstanceId == instanceId &&
                        item.Partition.PartitionX == x &&
                        item.Partition.PartitionY == y)
                    {
                        // Found it, return it. 
                        // logger.Debug("Returning cached partition: " + x + ", " + y);
                        item.LastReadAccess = DateTime.Now;
                        item.AccessCountInCache++;
                        return item;
                    }
                }

                // Ok, not in cache. We have to load and store it
                try
                {
                    this.sync.EnterWriteLock();
                    var cachePosition = this.FindFreePosition();

                    var partition = this.Database.LoadPartition(instanceId, x, y);
                    var cachedItem = new PartitionCacheItem()
                    {
                        AccessCountInCache = 1,
                        IsModified = false,
                        LastReadAccess = DateTime.Now,
                        Partition = partition
                    };

                    this.cachedPartitions[cachePosition] = cachedItem;
                    return cachedItem;
                }
                finally
                {
                    this.sync.ExitWriteLock();
                }
            }
            finally
            {
                this.sync.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Stores a certain partition on drive
        /// </summary>
        /// <param name="partition">Partition to be stored</param>
        public void StorePartition(Partition partition)
        {
            try
            {
                // First, find partition in cache. If partition is not in cache, we have to store the partition, but we do not
                // modify the cache
                this.sync.EnterWriteLock();

                for (var n = 0; n < this.CacheSize; n++)
                {
                    var item = this.cachedPartitions[n];

                    if (item != null &&
                        item.Partition.PartitionX == partition.PartitionX &&
                        item.Partition.PartitionY == partition.PartitionY)
                    {
                        // Nothing to do here. Saving will be done later
                        item.IsModified = true;
                        item.AccessCountInCache++;

                        // logger.LogEntry(LogLevel.Notify, "Stored in Cache: " + partition.ToString());
                        // Done
                        return;
                    }
                }

                logger.LogEntry(LogLevel.Notify, "Direct Store: " + partition.ToString());
                // Not in cache... (could have been removed while this the partition had been modified). Just store it
                this.Database.StorePartition(partition);
            }
            finally
            {
                this.sync.ExitWriteLock();
            }
        }

        /// <summary>
        /// Finds a free position in cache. This method may only 
        /// be called during write locking
        /// </summary>
        /// <returns>Free position</returns>
        private int FindFreePosition()
        {
            // Goes through cache and looks for free position, if no free position is found, 
            // find item was earliest access time
            var lastAccess = DateTime.MaxValue;
            var positionLastAccess = -1;
            for (var n = 0; n < this.CacheSize; n++)
            {
                var item = this.cachedPartitions[n];
                if (item == null)
                {
                    // logger.Debug("Found free cache entry: " + n);
                    return n;
                }

                if (lastAccess > item.LastReadAccess)
                {
                    // Stores the latest access
                    lastAccess = item.LastReadAccess;
                    positionLastAccess = n;
                }
            }

            // No free field found. 
            // Dispose oldest partition, store it to disc and stay happy!
            var oldestItem = this.cachedPartitions[positionLastAccess];
            if (oldestItem.IsModified)
            {
                logger.LogEntry(LogLevel.Notify, "Store of partition due to cache request: "
                    + oldestItem.Partition.ToString());
                this.Database.StorePartition(oldestItem.Partition);
            }

            // Empty field and return
            this.cachedPartitions[positionLastAccess] = null;
            return positionLastAccess;
        }

        /// <summary>
        /// Stores and clears all partitions
        /// </summary>
        public void StoreAndClearAll()
        {
            try
            {
                this.sync.EnterWriteLock();

                for (var n = 0; n < this.CacheSize; n++)
                {
                    var item = this.cachedPartitions[n];
                    if (item == null || !item.IsModified)
                    {
                        continue;
                    }

                    // Otherwise store
                    this.Database.StorePartition(item.Partition);

                    this.cachedPartitions[n] = null;
                }

                logger.LogEntry(LogLevel.Verbose, "StoreAndClearAll");
            }
            finally
            {
                this.sync.ExitWriteLock();
            }
        }

        /// <summary>
        /// Stores the info data
        /// </summary>
        public void StoreInfoData(long instanceId, VoxelMapInfo info)
        {
            try
            {
                this.sync.EnterWriteLock();

                this.infoCache[instanceId] = info;

                logger.LogEntry(LogLevel.Notify, "Storing VoxelMapInfo for instance: " + instanceId);
                this.Database.StoreInfoData(instanceId, info);
            }
            finally
            {
                this.sync.ExitWriteLock();
            }
        }

        /// <summary>
        /// Loads the info data from generic file
        /// </summary>
        /// <returns>Loaded info data</returns>
        public VoxelMapInfo LoadInfoData(long instanceId)
        {
            try
            {
                this.sync.EnterWriteLock();

                VoxelMapInfo result;
                if (this.infoCache.TryGetValue(instanceId, out result))
                {
                    return result;
                }

                result = this.Database.LoadInfoData(instanceId);
                this.infoCache[instanceId] = result;
                return result;
            }
            finally
            {
                this.sync.ExitWriteLock();
            }
        }

        /// <summary>
        /// Starts the runtime cache
        /// </summary>
        void IFlexBgRuntimeModule.Start()
        {
        }

        /// <summary>
        /// Stops the cache
        /// </summary>
        void IFlexBgRuntimeModule.Shutdown()
        {
            this.StoreAndClearAll();
        }
    }
}
