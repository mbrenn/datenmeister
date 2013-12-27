using BurnSystems.FlexBG.Modules.DeponNet.GameM;
using BurnSystems.FlexBG.Modules.DeponNet.GameM.Controllers;
using BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data;
using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM
{
    /// <summary>
    /// Data provider which assumes that the units are owned by a player and 
    /// player is owned by game
    /// </summary>
    public class UnitsInPlayerInGameDataProvider : IUnitDataProvider
    {
        /// <summary>
        /// Gets or sets the current game
        /// </summary>
        [Inject(ByName = DeponGamesController.CurrentGameName, IsMandatory = true)]
        public Game CurrentGame
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unit management
        /// </summary>
        [Inject(IsMandatory = true)]
        public IUnitManagement UnitManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the player management
        /// </summary>
        [Inject(IsMandatory = true)]
        public IPlayerManagement PlayerManagement
        {
            get;
            set;
        }

        /// <summary>
        /// Gets all units of the current game
        /// </summary>
        /// <returns>Enumeration of all units</returns>
        public IEnumerable<Unit> GetUnitsOfGame()
        {
            var players = this.PlayerManagement.GetPlayersOfGame(this.CurrentGame.Id);
            return players.SelectMany(x => this.UnitManagement.GetUnitsOfOwner(x.Id));
        }

        /// <summary>
        /// Gets all units within a certain region
        /// </summary>
        /// <param name="x1">Left X-Coordinate of the region</param>
        /// <param name="x2">Right X-Coordinate of the region</param>
        /// <param name="y1">Top Y-Coordinate of the region</param>
        /// <param name="y2">Bottom Y-Coordinate of the region</param>
        /// <returns>Enumeration of units</returns>
        public IEnumerable<Unit> GetUnitsInRegion(int x1, int x2, int y1, int y2)
        {
            return this.GetUnitsOfGame().Where(
                x =>
                    x.Position.X >= x1 &&
                    x.Position.X <= x2 &&
                    x.Position.Y >= y1 &&
                    x.Position.Y <= y2).ToList();
        }
    }
}
