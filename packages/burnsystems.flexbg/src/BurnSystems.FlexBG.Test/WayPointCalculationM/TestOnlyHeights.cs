using BurnSystems.FlexBG.Modules.ConfigurationStorageM;
using BurnSystems.FlexBG.Modules.DeponNet;
using BurnSystems.FlexBG.Modules.DeponNet.Common;
using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.MapM;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Generator;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.FlexBG.Modules.WayPointCalculationM;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Test.WayPointCalculationM
{
    [TestFixture]
    public class TestOnlyHeights
    {
        /// <summary>
        ///  Initialies the activation container
        /// </summary>
        /// <returns></returns>
        private IActivates Init(bool big = false)
        {
            if (!Directory.Exists("config"))
            {
                Assert.Inconclusive("No configuration File");
            }

            Log.TheLog.Reset();
            Log.TheLog.AddLogProvider(new DebugProvider());
            Log.TheLog.FilterLevel = LogLevel.Verbose;

            var container = new ActivationContainer("Test");

            var configurationStorage = new ConfigurationStorage();
            container.Bind<IConfigurationStorage>().ToConstant(configurationStorage);

            // VoxelMap
            BurnSystems.FlexBG.Modules.MapVoxelStorageM.Module.Load(container);

            container.Bind<IWayPointCalculation>().To<BurnSystems.FlexBG.Modules.WayPointCalculationM.OnlyHeight.WayPointCalculation>();

            var game = new Game();
            game.Id = 1;
            container.BindToName("CurrentGame").ToConstant(game);

            // Starts up FlexBG
            var runtime = new Runtime(container, "config");

            runtime.StartUpCore();

            // Create map
            var info = new VoxelMapInfo();
            info.SizeX = big ? 300 : 100;
            info.SizeY = big ? 300 : 100;
            info.PartitionLength = 100;

            var voxelMap = container.Get<IVoxelMap>();
            voxelMap.CreateMap(game.Id, info);
            CompleteFill.Execute(voxelMap, game.Id, GameConfig.Fields.Air.IdAsByte);
            new AddNoiseLayer(voxelMap, GameConfig.Fields.Grass.IdAsByte, () => 0, () => float.MinValue).Execute(game.Id);

            return container;
        }
        
        [Test]
        public void TestSetup()
        {
            var container = this.Init();
            var wayPointCalculation = container.Get<IWayPointCalculation>();

            var wayPoints = wayPointCalculation.CalculateWaypoints(
                new ObjectPosition(0, 0, 0),
                new ObjectPosition(50, 50, 50), 
                null);

            Assert.That(wayPoints, Is.Not.Null);
            Assert.That(wayPoints.Count() > 0);
        }

        [Test]
        public void TestLongSetup()
        {
            var container = this.Init(true);
            var wayPointCalculation = container.Get<IWayPointCalculation>();

            var wayPoints = wayPointCalculation.CalculateWaypoints(
                new ObjectPosition(0, 0, 0),
                new ObjectPosition(200, 200, 200),
                null);

            Assert.That(wayPoints, Is.Not.Null);
            Assert.That(wayPoints.Count() > 0);
        }

        [Test]
        public void TestBlockage()
        {
            var container = this.Init();

            var voxelMap = container.Get<IVoxelMap>();
            voxelMap.SetFieldType(1, 0, 2, GameConfig.Fields.Grass.IdAsByte, float.MinValue, float.MaxValue);
            voxelMap.SetFieldType(1, 1, 2, GameConfig.Fields.Grass.IdAsByte, float.MinValue, float.MaxValue);
            voxelMap.SetFieldType(1, 2, 2, GameConfig.Fields.Grass.IdAsByte, float.MinValue, float.MaxValue);
            voxelMap.SetFieldType(1, 2, 1, GameConfig.Fields.Grass.IdAsByte, float.MinValue, float.MaxValue);
            voxelMap.SetFieldType(1, 2, 0, GameConfig.Fields.Grass.IdAsByte, float.MinValue, float.MaxValue);

            var wayPointCalculation = container.Get<IWayPointCalculation>();

            var wayPoints = wayPointCalculation.CalculateWaypoints(
                new ObjectPosition(0, 0, 0),
                new ObjectPosition(5, 0, 0),
                null);

            Assert.That(wayPoints, Is.Null);
        }

        [Test]
        public void TestObstacles()
        {
            var container = this.Init();

            var voxelMap = container.Get<IVoxelMap>();
            voxelMap.SetFieldType(1, 0, 1, GameConfig.Fields.Grass.IdAsByte, float.MinValue, 100);
            voxelMap.SetFieldType(1, 1, 1, GameConfig.Fields.Grass.IdAsByte, float.MinValue, 100);
            voxelMap.SetFieldType(1, 3, 0, GameConfig.Fields.Grass.IdAsByte, float.MinValue, 100);
            voxelMap.SetFieldType(1, 3, 1, GameConfig.Fields.Grass.IdAsByte, float.MinValue, 100);
            voxelMap.SetFieldType(1, 5, 1, GameConfig.Fields.Grass.IdAsByte, float.MinValue, 100);

            var wayPointCalculation = container.Get<IWayPointCalculation>();

            var wayPoints = wayPointCalculation.CalculateWaypoints(
                new ObjectPosition(0, 0, 0),
                new ObjectPosition(5, 0, 0),
                null)
                .Select(x => new { X = x.X - 0.5, Y = x.Y - 0.5 })
                .ToList();

            Assert.That(wayPoints, Is.Not.Null);
            Assert.That(wayPoints.Count(), Is.EqualTo(10));

            Assert.That(wayPoints[0].X, Is.EqualTo(0));
            Assert.That(wayPoints[0].Y, Is.EqualTo(0));

            Assert.That(wayPoints[1].X, Is.EqualTo(1));
            Assert.That(wayPoints[1].Y, Is.EqualTo(0));
            
            Assert.That(wayPoints[2].X, Is.EqualTo(2));
            Assert.That(wayPoints[2].Y, Is.EqualTo(0));
            
            Assert.That(wayPoints[3].X, Is.EqualTo(2));
            Assert.That(wayPoints[3].Y, Is.EqualTo(1));
            
            Assert.That(wayPoints[4].X, Is.EqualTo(2));
            Assert.That(wayPoints[4].Y, Is.EqualTo(2));
            
            Assert.That(wayPoints[5].X, Is.EqualTo(3));
            Assert.That(wayPoints[5].Y, Is.EqualTo(2));
            
            Assert.That(wayPoints[6].X, Is.EqualTo(4));
            Assert.That(wayPoints[6].Y, Is.EqualTo(2));
            
            Assert.That(wayPoints[7].X, Is.EqualTo(4));
            Assert.That(wayPoints[7].Y, Is.EqualTo(1));
            
            Assert.That(wayPoints[8].X, Is.EqualTo(4));
            Assert.That(wayPoints[8].Y, Is.EqualTo(0));

            Assert.That(wayPoints[9].X, Is.EqualTo(5));
            Assert.That(wayPoints[9].Y, Is.EqualTo(0));

            var wayPoints2 = wayPointCalculation.CalculateWaypoints(
                new ObjectPosition(5, 0, 0),
                new ObjectPosition(0, 0, 0),
                null)
                .Select(x => new { X = x.X - 0.5, Y = x.Y - 0.5 })
                .ToList();

            Assert.That(wayPoints2, Is.Not.Null);
            Assert.That(wayPoints2.Count(), Is.EqualTo(10));

            Assert.That(wayPoints2[9].X, Is.EqualTo(0));
            Assert.That(wayPoints2[9].Y, Is.EqualTo(0));

            Assert.That(wayPoints2[8].X, Is.EqualTo(1));
            Assert.That(wayPoints2[8].Y, Is.EqualTo(0));

            Assert.That(wayPoints2[7].X, Is.EqualTo(2));
            Assert.That(wayPoints2[7].Y, Is.EqualTo(0));

            Assert.That(wayPoints2[6].X, Is.EqualTo(2));
            Assert.That(wayPoints2[6].Y, Is.EqualTo(1));

            Assert.That(wayPoints2[5].X, Is.EqualTo(2));
            Assert.That(wayPoints2[5].Y, Is.EqualTo(2));

            Assert.That(wayPoints2[4].X, Is.EqualTo(3));
            Assert.That(wayPoints2[4].Y, Is.EqualTo(2));

            Assert.That(wayPoints2[3].X, Is.EqualTo(4));
            Assert.That(wayPoints2[3].Y, Is.EqualTo(2));

            Assert.That(wayPoints2[2].X, Is.EqualTo(4));
            Assert.That(wayPoints2[2].Y, Is.EqualTo(1));

            Assert.That(wayPoints2[1].X, Is.EqualTo(4));
            Assert.That(wayPoints2[1].Y, Is.EqualTo(0));

            Assert.That(wayPoints2[0].X, Is.EqualTo(5));
            Assert.That(wayPoints2[0].Y, Is.EqualTo(0));
        }
    }
}
