using BurnSystems.FlexBG.Helper;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.MapM.Admin;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*namespace BurnSystems.FlexBG.Test.MapVoxelStorage
{
    [TestFixture]
    public class CompleteGameTests
    {
        [Test]
        public void TestLoadingAndStoring()
        {
            Log.TheLog.Reset();
            Log.TheLog.AddLogProvider(new DebugProvider());
            Log.TheLog.FilterLevel = LogLevel.Verbose;

            SerializedFile.ClearCompleteDataDirectory();
            long gameId;

            // Creates the game
            {
                var activationContainer = new ActivationContainer("Game");
                Init.CreateGameLogic(activationContainer, false);

                var runtime = new Runtime(activationContainer, "config");

                runtime.StartUpCore();

                // Creates game
                var controller = activationContainer.Create<DeponGamesAdminController>();
                gameId = controller.CreateGame(
                    new Modules.DeponNet.GameM.Admin.DeponCreateGameModel()
                    {
                        Title = "Test",
                        Description = "Description",
                        MaxPlayers = 10,
                        MapHeight = 200,
                        MapWidth = 200
                    });

                runtime.ShutdownCore();
            }

            // Loads the game
            {
                var activationContainer = new ActivationContainer("Game");
                Init.CreateGameLogic(activationContainer, false);

                var runtime = new Runtime(activationContainer, "config");

                runtime.StartUpCore();

                var voxelMap = activationContainer.Get<IVoxelMap>();
                var surfaceInfo = voxelMap.GetSurfaceInfo(gameId, 0, 0, 10, 10);
                for (var x = 0; x < 10; x++)
                {
                    for (var y = 0; y < 10; y++)
                    {
                        var fieldType = surfaceInfo[x][y].FieldType;
                        Assert.IsTrue(fieldType == 1 || fieldType == 2, "Field Type is not 1 or 2");
                    }
                }

                runtime.ShutdownCore();
            }
        }
    }
}
*/