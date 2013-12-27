using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using System.IO;
using BurnSystems.Logging;

namespace BurnSystems.FlexBG.Test.MapVoxelStorage
{
    [TestFixture]
    public class DatabaseCacheTests
    {
        [Test]
        public void TestSimpleCacheLoading()
        {
            var info = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 10000,
                SizeY = 20000
            };

            var database = new PartitionLoader(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();

            var cache = new PartitionCache(database, 5);
            cache.StoreInfoData(0, info);

            DatabaseTests.PerformDatabaseLoaderTests(cache);
        }

        [Test]
        public void TestRetrievalFromCache()
        {
            var info = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 10000,
                SizeY = 20000
            };

            var database = new PartitionLoader(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            database.StoreInfoData(0, info);

            var cache = new PartitionCache(database, 5);

            var partition1 = new Partition(0, 0, 0, 100);
            var partition2 = new Partition(0, 0, 1, 100);
            partition1.InitFields();
            partition2.InitFields();

            partition1.SetFieldType(1, 5, 2, 100, 50);
            partition2.SetFieldType(2, 1, 3, 100, 50);

            cache.StorePartition(partition1);
            cache.StorePartition(partition2);

            var partition1Loaded = cache.LoadPartition(0, 0, 0);
            var partition2Loaded1 = cache.LoadPartition(0, 0, 1);
            var partition2Loaded2 = cache.LoadPartition(0, 0, 1);

            Assert.That(partition1Loaded.GetFieldType(0, 1, 75), Is.EqualTo(0));
            Assert.That(partition1Loaded.GetFieldType(1, 5, 75), Is.EqualTo(2));
            Assert.That(partition2Loaded1.GetFieldType(0, 1, 75), Is.EqualTo(0));
            Assert.That(partition2Loaded1.GetFieldType(2, 1, 75), Is.EqualTo(3));
            Assert.That(partition2Loaded2.GetFieldType(2, 1, 75), Is.EqualTo(3));

            // Stores partition, should only be stored in cache
            partition2Loaded1.SetFieldType(5, 6, 4, 100, 1000);
            cache.StorePartition(partition2Loaded1);

            var notCachedPartition = database.LoadPartition(0, 0, 1);
            Assert.That(partition2Loaded1.GetFieldType(5, 6, 500), Is.EqualTo(4));
            Assert.That(notCachedPartition.GetFieldType(5, 6, 500), Is.EqualTo(0));

            var partition2Loaded3 = cache.LoadPartition(0, 0, 1);
            Assert.That(partition2Loaded3.GetFieldType(5, 6, 500), Is.EqualTo(4));

            // Clears cache
            cache.StoreAndClearAll();
            var notCachedPartition2 = database.LoadPartition(0, 0, 1);
            Assert.That(notCachedPartition2.GetFieldType(5, 6, 500), Is.EqualTo(4));
        }

        [Test]
        public void TestFillCache()
        {
            var info = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 10000,
                SizeY = 20000
            };

            var database = new PartitionLoader(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();

            var cache = new PartitionCache(database, 5);
            database.StoreInfoData(0, info);

            var partitions = new Partition[10];
            for (var n = 0; n < 10; n++)
            {
                partitions[n] = cache.LoadPartition(0, n, 2);
                partitions[n].InitFields();
                partitions[n].SetFieldType(n, n + 1, (byte) (n + 2), 500, 1000);

                cache.StorePartition(partitions[n]);
            }

            var log = new ClassLogger(typeof(DatabaseCacheTests));
            log.LogEntry(LogLevel.Verbose, "Finished writing, now reading");

            var loadedPartitions = new Partition[10];
            for (var n = 0; n < 10; n++)
            {
                loadedPartitions[n] = cache.LoadPartition(0, n, 2);
                Assert.That(partitions[n].GetFieldType(n, n + 1, 700), Is.EqualTo((byte)(n + 2)));
            }
        }
    }
}
