using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interfaces;
using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.TownM;
using BurnSystems.FlexBG.Modules.DeponNet.TownM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
using BurnSystems.FlexBG.Modules.LockMasterM;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Modules.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.MapM.Controllers
{
    /// <summary>
    /// Returns all game objects in a certain region
    /// </summary>
    public class GameObjectsController : Controller
    {
        [Inject(ByName = DeponGamesController.CurrentGameName, IsMandatory = true)]
        public Game CurrentGame
        {
            get;
            set;
        }

        [Inject(ByName = DeponPlayersController.CurrentPlayerName, IsMandatory=true)]
        public Player CurrentPlayer
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IBuildingDataProvider BuildingDataProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Gets orsets the unit data provider
        /// </summary>
        [Inject(IsMandatory = true)]
        public IUnitDataProvider UnitDataProvider
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }

        [Inject]
        public ITownManagement TownManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IUnitManagement UnitManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IBuildingTypeProvider BuildingTypeProvider
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IUnitTypeProvider UnitTypeProvider
        {
            get;
            set;
        }

        [Inject(IsMandatory=true)]
        public ILockMaster LockMaster
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets the gameobjects on the map
        /// </summary>
        /// <param name="x1">Left-X-Coordinate of the map</param>
        /// <param name="x2">Right-X-Coordinate of the map</param>
        /// <param name="y1">Top-Y-Coordinate of the map</param>
        /// <param name="y2">Bottom-Y-Coordinate of the map</param>
        /// <returns>Enumeration of gameobjects</returns>
        [WebMethod]
        public IActionResult GetGameObjects(int x1, int x2, int y1, int y2)
        {
            using (this.LockMaster.AcquireReadLock())
            {
                var gameObjects = new List<object>();
                this.ListBuildings(x1, x2, y1, y2, gameObjects);
                this.ListUnits(x1, x2, y1, y2, gameObjects);

                var result = new
                {
                    success = true,
                    gameObjects = gameObjects
                };

                return this.Json(result);
            }
        }

        /// <summary>
        /// Lists all buildings within the given region 
        /// </summary>
        /// <param name="x1">Left Coordinate</param>
        /// <param name="x2">Right Coordinate</param>
        /// <param name="y1">Top Coordinate</param>
        /// <param name="y2">Bottom Coordinate</param>
        /// <param name="gameObjects">List of game objects, where buildings will be attached</param>
        private void ListBuildings(int x1, int x2, int y1, int y2, List<object> gameObjects)
        {
            var buildings = this.BuildingDataProvider.GetBuildingsInRegion(x1, x2, y1, y2);
            foreach (var building in buildings)
            {
                var player = this.PlayerManagement.GetPlayer(building.PlayerId) ?? new Player();
                var town = this.TownManagement.GetTown(building.TownId) ?? new Town();

                gameObjects.Add(
                    new
                    {
                        id = building.Id,
                        bildingTypeId = building.BuildingTypeId,
                        buildingType = this.BuildingTypeProvider.Get(building.BuildingTypeId).Token,
                        lvel = building.Level,
                        townId = building.TownId,
                        playerId = building.PlayerId,
                        playername = player.Playername,
                        townname = town.TownName,
                        gameObjectType = "building",
                        x = building.Position.X,
                        y = building.Position.Y,
                        t = building.Position.Z
                    });
            }
        }

        /// <summary>
        /// Lists all units within the game objects
        /// </summary>
        /// <param name="x1">Left Coordinate</param>
        /// <param name="x2">Right Coordinate</param>
        /// <param name="y1">Top Coordinate</param>
        /// <param name="y2">Bottom Coordinate</param>
        /// <param name="gameObjects">List of game objects, where buildings will be attached</param>
        private void ListUnits(int x1, int x2, int y1, int y2, List<object> gameObjects)
        {
            var units = this.UnitDataProvider.GetUnitsInRegion(x1, x2, y1, y2);
            foreach (var unit in units)
            {
                var player = this.PlayerManagement.GetPlayer(unit.OwnerId) ?? new Player();

                gameObjects.Add(
                    new
                    {
                        id = unit.Id,
                        unitTypeId = unit.UnitTypeId,
                        unitType = this.UnitTypeProvider.Get(unit.UnitTypeId).Token,
                        amount = unit.Amount,
                        playerId = player.Id,
                        playername = player.Playername,
                        gameObjectType = "unit",
                        x = unit.Position.X,
                        y = unit.Position.Y,
                        t = unit.Position.Z
                    });
            }
        }
    }
}
