using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.Test;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Test.MapVoxelStorage
{
    [TestFixture]
    public class DataFieldTests
    {
        private static byte[] testData = new byte[] { 2, 5, 6, 8 };

        private static byte[] testData2 = new byte[] { 12, 55, 33, 8, 10 };

        [Test]
        public void TestSetAndGet()
        {
            var map = VoxelMapTests.CreateMap();
            map.SetData(0, 10, 10, 1, testData);
            map.SetData(0, 11, 11, 1, testData2);

            var retrievedData = map.GetData(0, 10, 10, 1);
            var retrievedData2 = map.GetData(0, 11, 11, 1);

            CheckData(retrievedData, testData);
            CheckData(retrievedData2, testData2);
            
            var retrievedData3 = map.GetData(0, 2, 2, 1);
            Assert.That(retrievedData3, Is.Null);

            (map.Loader as PartitionCache).StoreAndClearAll();
        }

        [Test]
        public void TestSetSetAndGet()
        {
            var map = VoxelMapTests.CreateMap();
            
            map.SetData(0, 10, 10, 1, testData);

            var retrievedData = map.GetData(0, 10, 10, 1);
            CheckData(retrievedData, testData);

            map.SetData(0, 10, 10, 1, testData2);
            retrievedData = map.GetData(0, 10, 10, 1);
            CheckData(retrievedData, testData2);

            (map.Loader as PartitionCache).StoreAndClearAll();
        }

        [Test]
        public void TestSetStoreAndGet()
        {
            var map = VoxelMapTests.CreateMap();
            map.SetData(0, 10, 10, 1, testData);

            var retrievedData = map.GetData(0, 10, 10, 1);
            CheckData(retrievedData, testData);

            map.SetData(0, 10, 10, 1, testData2);
            retrievedData = map.GetData(0, 10, 10, 1);
            CheckData(retrievedData, testData2);

            (map.Loader as PartitionCache).StoreAndClearAll();

            var map2 = VoxelMapTests.LoadMap();
            retrievedData = map2.GetData(0, 10, 10, 1);
            CheckData(retrievedData, testData2);

            (map.Loader as PartitionCache).StoreAndClearAll();
        }

        /// <summary>
        /// Tests whether the retrieved data is matching to testData
        /// </summary>
        /// <param name="otherData"></param>
        private static void CheckData(byte[] otherData, byte[] dataToBeTested)
        {
            Assert.That(otherData, Is.Not.Null);
            Assert.That(otherData.Length == dataToBeTested.Length);
            for (var x = 0; x < otherData.Length; x++)
            {
                Assert.That(otherData[x] == dataToBeTested[x]);
            }
        }
    }
}
