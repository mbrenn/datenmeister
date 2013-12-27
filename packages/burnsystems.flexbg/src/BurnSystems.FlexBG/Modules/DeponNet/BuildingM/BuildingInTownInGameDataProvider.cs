using BurnSystems.FlexBG.Modules.DeponNet.BuildingM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.TownM.Interface;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.BuildingM
{
    /// <summary>
    /// Dataprovider returning building information where building are assumed as located
    /// in towns and towns are located in games
    /// </summary>
    public class BuildingInTownInGameDataProvider : IBuildingDataProvider
    {
        [Inject(IsMandatory = true)]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true )]
        public ITownManagement TownManagement
        {
            get;
            set;
        }

        [Inject(IsMandatory = true)]
        public IBuildingManagement BuildingManagement
        {
            get;
            set;
        }

        [Inject(ByName = DeponGamesController.CurrentGameName, IsMandatory = true)]
        public Game CurrentGame
        {
            get;
            set;
        }

        /// <summary>
        /// Gets all buildings in a certain region
        /// </summary>
        /// <param name="x1">Left X-Coordinate in the map</param>
        /// <param name="x2">Right X-Coordinate in the map</param>
        /// <param name="y1">Top Y-Coordinate in the map</param>
        /// <param name="y2">Bottom Y-Coordinate in the map</param>
        /// <returns>Enumeration of buildings</returns>
        public IEnumerable<Building> GetBuildingsInRegion(int x1, int x2, int y1, int y2)
        {
            var buildings = this.GetAllBuildings();

            return buildings.Where(
                x =>
                    x.Position.X >= x1 &&
                    x.Position.X <= x2 &&
                    x.Position.Y >= y1 &&
                    x.Position.Y <= y2).ToList();
        }

        /// <summary>
        /// Gets all buildings of game
        /// </summary>
        /// <returns>Enumeration of buildings</returns>
        public IEnumerable<Building> GetAllBuildings()
        {
            var gameId = this.CurrentGame.Id;
            var players = this.PlayerManagement.GetPlayersOfGame(gameId);
            var towns = players.SelectMany(x => this.TownManagement.GetTownsOfPlayer(x.Id));
            var buildings = towns.SelectMany(x => this.BuildingManagement.GetBuildingsOfTown(x.Id));
            return buildings;
        }

        /// <summary>
        /// Gets the buildings of the player
        /// </summary>
        /// <param name="playerId">Id of the player</param>
        /// <returns>Enumeration of buildings</returns>
        public IEnumerable<Building> GetBuildingsOfPlayer(long playerId)
        {
            var towns = this.TownManagement.GetTownsOfPlayer(playerId);
            return towns.SelectMany(x => this.BuildingManagement.GetBuildingsOfTown(x.Id));
        }
    }
}
