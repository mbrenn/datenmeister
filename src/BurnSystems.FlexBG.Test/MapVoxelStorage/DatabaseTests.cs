using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using System.IO;

namespace BurnSystems.FlexBG.Test.MapVoxelStorage
{
    /// <summary>
    /// Defines tests for databases tests
    /// </summary>
    [TestFixture]
    public class DatabaseTests
    {
        /// <summary>
        /// Tests the loading and storing of a partition
        /// </summary>
        [Test]
        public void TestInitializationOfPartition()
        {
            var partition = new Partition(0, 1, 1, 100);
            partition.InitFields();
            for (var x = 0; x < 99; x++)
            {
                for (var y = 0; y < 99; y++)
                {
                    Assert.That(partition.GetFieldType(x, y, -500), Is.EqualTo(0));
                    Assert.That(partition.GetFieldType(x, y, 500), Is.EqualTo(0));
                    Assert.That(partition.GetFieldType(x, y, 0), Is.EqualTo(0));
                }
            }
        }

        /// <summary>
        /// Tests the loading and storing of a partition
        /// </summary>
        [Test]
        public void TestLoadingAndStoring()
        {
            var partition = new Partition(0, 1, 1, 100);
            partition.InitFields();
            partition.SetFieldType(2, 2, 1, 1000, 50);
            partition.SetFieldType(5, 20, 2, 2000, 1000);

            byte[] byteBuffer;

            // Store into stream
            using (var memoryStream = new MemoryStream())
            {
                partition.Store(memoryStream);
                var localByteBuffer = memoryStream.GetBuffer();
                var length = memoryStream.Length;

                byteBuffer = new byte[length];
                Array.Copy(localByteBuffer, byteBuffer, length);
            }

            // Partition creation
            var partitionLoad = new Partition(0, 1, 1, 100);

            // Load stream
            using (var memoryStream = new MemoryStream(byteBuffer))
            {
                partitionLoad.Load(memoryStream);
            }

            Assert.That(partitionLoad.GetFieldType(0, 0, 0), Is.EqualTo(0));
            Assert.That(partitionLoad.GetFieldType(2, 2, 0), Is.EqualTo(0));
            Assert.That(partitionLoad.GetFieldType(2, 2, 500), Is.EqualTo(1));
            Assert.That(partitionLoad.GetFieldType(5, 20, 0), Is.EqualTo(0));
            Assert.That(partitionLoad.GetFieldType(5, 20, 1800), Is.EqualTo(2));
            Assert.That(partitionLoad.GetFieldType(5, 20, 1100), Is.EqualTo(2));
            Assert.That(partitionLoad.GetFieldType(5, 20, 3000), Is.EqualTo(0));
        }

        [Test]
        public void TestConversion()
        {
            var partition = new Partition(0, 3, 1, 100);
            int absX, absY;
            partition.ConvertToAbsolute(5, 2, out absX, out absY);

            Assert.That(absX, Is.EqualTo(305));
            Assert.That(absY, Is.EqualTo(102));

            int relX, relY;

            partition.ConvertToRelative(absX, absY, out relX, out relY);
            Assert.That(relX, Is.EqualTo(5));
            Assert.That(relY, Is.EqualTo(2));
        }

        [Test]
        public void TestPartitionLoader()
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

            PerformDatabaseLoaderTests(database);
        }

        public static void PerformDatabaseLoaderTests(IPartitionLoader database)
        {
            var partition1 = new Partition(0, 0, 0, 100);
            var partition2 = new Partition(0, 1, 0, 100);
            var partition3 = new Partition(0, 1, 1, 100);
            var partition4 = new Partition(0, 0, 1, 100);

            partition1.InitFields();
            partition2.InitFields();
            partition3.InitFields();
            partition4.InitFields();
            partition1.SetFieldType(3, 1, 2, 100, 50);
            partition2.SetFieldType(3, 2, 3, 100, 50);
            partition3.SetFieldType(3, 4, 4, 100, 50);
            partition4.SetFieldType(3, 5, 5, 100, 50);

            database.StorePartition(partition1);
            database.StorePartition(partition2);
            database.StorePartition(partition3);
            database.StorePartition(partition4);

            var partitionLoad1 = database.LoadPartition(0, 0, 0);
            var partitionLoad2 = database.LoadPartition(0, 1, 0);
            var partitionLoad3 = database.LoadPartition(0, 1, 1);
            var partitionLoad4 = database.LoadPartition(0, 0, 1);
            var partitionLoad5 = database.LoadPartition(0, 2, 1);

            var infoDataLoaded = database.LoadInfoData(0);

            Assert.That(infoDataLoaded.PartitionLength, Is.EqualTo(100));
            Assert.That(infoDataLoaded.SizeX, Is.EqualTo(10000));
            Assert.That(infoDataLoaded.SizeY, Is.EqualTo(20000));

            Assert.That(partitionLoad1.GetFieldType(0, 0, 75), Is.EqualTo(0));
            Assert.That(partitionLoad1.GetFieldType(3, 1, 75), Is.EqualTo(2));

            Assert.That(partitionLoad2.GetFieldType(0, 0, 75), Is.EqualTo(0));
            Assert.That(partitionLoad2.GetFieldType(3, 2, 75), Is.EqualTo(3));

            Assert.That(partitionLoad3.GetFieldType(0, 0, 75), Is.EqualTo(0));
            Assert.That(partitionLoad3.GetFieldType(3, 4, 75), Is.EqualTo(4));

            Assert.That(partitionLoad4.GetFieldType(0, 0, 75), Is.EqualTo(0));
            Assert.That(partitionLoad4.GetFieldType(3, 5, 75), Is.EqualTo(5));

            Assert.That(partitionLoad5.GetFieldType(0, 0, 75), Is.EqualTo(0));
            Assert.That(partitionLoad5.GetFieldType(3, 1, 75), Is.EqualTo(0));
        }
    }
}
