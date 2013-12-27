using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using System.IO;

namespace BurnSystems.FlexBG.Test.MapVoxelStorage
{
    [TestFixture]
    public class MapGeneratorTests
    {
        public static VoxelMap CreateVoxelMap()
        {
            var info = new VoxelMapInfo()
            {
                PartitionLength = 100,
                SizeX = 1000,
                SizeY = 1000
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

            return voxelMap;
        }

        /// <summary>
        /// Tests the creation of ground on map
        /// </summary>
        [Test]
        public void TestGroundCreation()
        {

        }
    }
}
