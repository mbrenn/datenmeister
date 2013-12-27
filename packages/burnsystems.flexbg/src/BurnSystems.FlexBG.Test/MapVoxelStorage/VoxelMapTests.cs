using System;
using System.IO;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using NUnit.Framework;

namespace BurnSystems.FlexBG.Test.MapVoxelStorage
{
    [TestFixture]
    public class VoxelMapTests
    {
        [Test]
        public void TestMapCreation()
        {
            var voxelMap = CreateMap();

            for (var x = 0; x < 80; x += 10)
            {
                Assert.That(voxelMap.GetFieldType(0, x, x / 2, 100), Is.EqualTo(0));
            }
        }

        [Test]
        public void TestMapRetrieval()
        {
            var info = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 500,
                SizeY = 500
            };

            var database = new PartitionLoader(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            var cache = new PartitionCache(database);
            var voxelMap = new VoxelMap();
            voxelMap.Loader = cache;
            voxelMap.CreateMap(0, info);

            for (var x = 0; x < info.SizeX; x ++)
            {
                for (var y = 0; y < info.SizeY; y++)
                {
                    voxelMap.SetFieldType(0, x, y, 2, ((float)x) * ((float)y) + 1, 0);
                }
            }

            for (var x = 0; x < info.SizeX; x++)
            {
                for (var y = 0; y < info.SizeY; y++)
                {
                    Assert.That(voxelMap.GetFieldType(0, x, y, (((float)x) * ((float)y) + 1) / 2), Is.EqualTo(2));
                    Assert.That(voxelMap.GetFieldType(0, x, y, (((float)x) * ((float)y) + 1) * 2), Is.EqualTo(0));
                }
            }
        }

        [Test]
        public void TestMapSurface()
        {
            var info = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 500,
                SizeY = 500
            };

            var database = new PartitionLoader(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            var cache = new PartitionCache(database);
            var voxelMap = new VoxelMap();
            voxelMap.Loader = cache;
            voxelMap.CreateMap(0, info);

            for (var x = 0; x < info.SizeX; x++)
            {
                for (var y = 0; y < info.SizeY; y++)
                {
                    voxelMap.SetFieldType(0, x, y, 2, ((float)x) * ((float)y) + 1, 0);
                }
            }

            var surfaceInfo = voxelMap.GetSurfaceInfo(0, 0, 0, 200, 200);

            for (var x = 0; x < 200; x++)
            {
                for (var y = 0; y < 200; y++)
                {
                    Assert.That(surfaceInfo[x][y].ChangeHeight, Is.EqualTo(x * y + 1));
                    Assert.That(surfaceInfo[x][y].FieldType, Is.EqualTo(2));
                }
            }

            var surfaceInfo2 = voxelMap.GetSurfaceInfo(0, 200, 200, 400, 400);

            for (var x = 0; x < 200; x++)
            {
                for (var y = 0; y < 200; y++)
                {
                    Assert.That(surfaceInfo2[x][y].ChangeHeight, Is.EqualTo((200 + x) * (200 + y) + 1));
                    Assert.That(surfaceInfo2[x][y].FieldType, Is.EqualTo(2));
                }
            }
        }

        [Test]
        public void TestCreateBigMap()
        {
            var info = new VoxelMapInfo()
            {
                PartitionLength = 250,
                SizeX = 2000,
                SizeY = 2000
            };

            var database = new PartitionLoader(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            var cache = new PartitionCache(database);
            var voxelMap = new VoxelMap();
            voxelMap.Loader = cache;
            voxelMap.CreateMap(0, info);
        }

        [Test]
        public void TestInstanceInfosWithoutCache()
        {
            var info1 = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 500,
                SizeY = 500
            };

            var info2 = new VoxelMapInfo()
            {
                PartitionLength = 50,
                SizeX = 500,
                SizeY = 500
            };

            var database = new PartitionLoader(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            //var cache = new PartitionCache(database, 5);

            var voxelMap = new VoxelMap();
            voxelMap.Loader = database;

            voxelMap.CreateMap(0, info1);
            voxelMap.CreateMap(1, info2);

            var newInfo1 = database.LoadInfoData(0);
            var newInfo2 = database.LoadInfoData(1);

            Assert.That(newInfo1.PartitionLength, Is.EqualTo(info1.PartitionLength));
            Assert.That(newInfo1.SizeX, Is.EqualTo(info1.SizeX));
            Assert.That(newInfo1.SizeY, Is.EqualTo(info1.SizeY));

            Assert.That(newInfo2.PartitionLength, Is.EqualTo(info2.PartitionLength));
            Assert.That(newInfo2.SizeX, Is.EqualTo(info2.SizeX));
            Assert.That(newInfo2.SizeY, Is.EqualTo(info2.SizeY));
        }

        [Test]
        public void TestInstanceInfosWithCache()
        {
            var info1 = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 500,
                SizeY = 500
            };

            var info2 = new VoxelMapInfo()
            {
                PartitionLength = 50,
                SizeX = 500,
                SizeY = 500
            };

            var database = new PartitionLoader(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            var cache = new PartitionCache(database, 5);

            var voxelMap = new VoxelMap();
            voxelMap.Loader = database;

            voxelMap.CreateMap(0, info1);
            voxelMap.CreateMap(1, info2);

            var newInfo1 = cache.LoadInfoData(0);
            var newInfo2 = cache.LoadInfoData(1);

            Assert.That(newInfo1.PartitionLength, Is.EqualTo(info1.PartitionLength));
            Assert.That(newInfo1.SizeX, Is.EqualTo(info1.SizeX));
            Assert.That(newInfo1.SizeY, Is.EqualTo(info1.SizeY));

            Assert.That(newInfo2.PartitionLength, Is.EqualTo(info2.PartitionLength));
            Assert.That(newInfo2.SizeX, Is.EqualTo(info2.SizeX));
            Assert.That(newInfo2.SizeY, Is.EqualTo(info2.SizeY));
        }

        [Test]
        public void TestFieldTypeWithoutCache()
        {
            var info1 = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 500,
                SizeY = 500
            };

            var info2 = new VoxelMapInfo()
            {
                PartitionLength = 50,
                SizeX = 500,
                SizeY = 500
            };

            var database = new PartitionLoader(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            //var cache = new PartitionCache(database, 5);

            var voxelMap = new VoxelMap();
            voxelMap.Loader = database;

            voxelMap.CreateMap(0, info1);
            voxelMap.CreateMap(1, info2);

            voxelMap.SetFieldType(0, 1, 1, 1, 20, 50);
            voxelMap.SetFieldType(1, 1, 1, 2, 20, 50);


            var fieldType1 = voxelMap.GetFieldType(0, 1, 1, 30);
            var fieldType2 = voxelMap.GetFieldType(1, 1, 1, 30);

            Assert.That(fieldType1, Is.EqualTo(1));
            Assert.That(fieldType2, Is.EqualTo(2));
        }

        [Test]
        public void TestFieldTypeWithCache()
        {
            var info1 = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 500,
                SizeY = 500
            };

            var info2 = new VoxelMapInfo()
            {
                PartitionLength = 50,
                SizeX = 500,
                SizeY = 500
            };

            var database = new PartitionLoader(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            var cache = new PartitionCache(database, 5);

            var voxelMap = new VoxelMap();
            voxelMap.Loader = cache;

            voxelMap.CreateMap(0, info1);
            voxelMap.CreateMap(1, info2);

            voxelMap.SetFieldType(0, 1, 1, 1, 20, 50);
            voxelMap.SetFieldType(1, 1, 1, 2, 20, 50);


            var fieldType1 = voxelMap.GetFieldType(0, 1, 1, 30);
            var fieldType2 = voxelMap.GetFieldType(1, 1, 1, 30);

            Assert.That(fieldType1, Is.EqualTo(1));
            Assert.That(fieldType2, Is.EqualTo(2));
        }

        /// <summary>
        /// Create a map that can be used for testing
        /// </summary>
        /// <returns></returns>
        public static VoxelMap CreateMap()
        {
            var info = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 100,
                SizeY = 200
            };

            var database = new PartitionLoader(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            database.Clear();
            var cache = new PartitionCache(database, 5);

            var voxelMap = new VoxelMap();
            voxelMap.Loader = cache;

            voxelMap.CreateMap(0, info);
            return voxelMap;
        }

        /// <summary>
        /// Create a map that can be used for testing
        /// </summary>
        /// <returns></returns>
        public static VoxelMap LoadMap()
        {
            var database = new PartitionLoader(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "MapTests"));
            var cache = new PartitionCache(database, 5);

            var voxelMap = new VoxelMap();
            voxelMap.Loader = cache;
            return voxelMap;
        }
    }
}
