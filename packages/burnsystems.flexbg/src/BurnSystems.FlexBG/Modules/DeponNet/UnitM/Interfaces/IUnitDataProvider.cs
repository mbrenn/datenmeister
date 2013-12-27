using BurnSystems.FlexBG.Modules.DeponNet.UnitM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.UnitM.Interfaces
{
    /// <summary>
    /// The data provider for the units
    /// </summary>
    public interface IUnitDataProvider
    {
        /// <summary>
        /// Gets all units within a game. 
        /// </summary>
        /// <param name="gameId">Id of the game</param>
        /// <returns></returns>
        IEnumerable<Unit> GetUnitsOfGame();

        /// <summary>
        /// Gets all units within a certain region
        /// </summary>
        /// <param name="x1">Left-Coordinate of the units</param>
        /// <param name="x2">Right-Coordinate of the units</param>
        /// <param name="y1">Top-Coordinate of the units</param>
        /// <param name="y2">Bottom-Coordinate of the units</param>
        /// <returns>Enumeration of units</returns>
        IEnumerable<Unit> GetUnitsInRegion(int x1, int x2, int y1, int y2);
    }
}
