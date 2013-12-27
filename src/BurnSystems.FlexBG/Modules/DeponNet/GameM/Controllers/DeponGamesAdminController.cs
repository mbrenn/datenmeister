using BurnSystems.FlexBG.Modules.DeponNet.GameM.Admin;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.MapM;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Generator;
using BurnSystems.FlexBG.Modules.MapVoxelStorageM.Storage;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers
{
    public class DeponGamesAdminController : Controller
    {
        private static ILog logger = new ClassLogger(typeof(DeponGameAdminInterface));

        /// <summary>
        /// Gets or sets the game management
        /// </summary>
        [Inject(IsMandatory = true)]
        public IGameManagement GameManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IVoxelMap VoxelMap
        {
            get;
            set;
        }

        [WebMethod]
        public IActionResult Create([PostModel] DeponCreateGameModel model)
        {
            CreateGame(model);
            return this.SuccessJson();
        }

        /// <summary>
        /// Creates game and returns the game id
        /// </summary>
        /// <param name="model">Model to be used</param>
        public long CreateGame(DeponCreateGameModel model)
        {
            var gameId = this.GameManagement.Create(model.Title, model.Description, model.MaxPlayers);

            var info = new VoxelMapInfo();
            info.SizeX = model.MapWidth;
            info.SizeY = model.MapHeight;
            info.PartitionLength = 100;

            logger.LogEntry(LogLevel.Notify, "Create Map for game " + gameId.ToString());

            this.VoxelMap.CreateMap(
                gameId,
                info);

            logger.LogEntry(LogLevel.Notify, "Fill Map for game " + gameId.ToString());
            CompleteFill.Execute(this.VoxelMap, gameId, GameConfig.Fields.Air.IdAsByte);

            logger.LogEntry(LogLevel.Notify, "Add Grass for game " + gameId.ToString());
            new AddNoiseLayer(this.VoxelMap, GameConfig.Fields.Grass.IdAsByte, () => 0, () => float.MinValue).Execute(gameId);

            logger.LogEntry(LogLevel.Notify, "Add DarkGrass for game " + gameId.ToString());
            new ReplaceFieldType(this.VoxelMap, GameConfig.Fields.Grass.IdAsByte, GameConfig.Fields.DarkGrass.IdAsByte, (x, y) => MathHelper.Random.NextDouble() < 0.1).Execute(gameId);

            logger.LogEntry(LogLevel.Notify, "Finished map creation for game " + gameId.ToString());

            return gameId;
        }
    }
}
