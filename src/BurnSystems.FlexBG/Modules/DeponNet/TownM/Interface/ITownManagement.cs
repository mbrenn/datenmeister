using BurnSystems.FlexBG.Modules.DeponNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.TownM.Interface
{
    /// <summary>
    /// Interface for town management
    /// </summary>
    public interface ITownManagement
    {
        /// <summary>
        /// Creates a new town
        /// </summary>
        /// <param name="playerId">Id of the player, who is the owner of the town</param>
        /// <param name="townName">Name of the town</param>
        /// <param name="isCapital">true, if this is the capital</param>
        /// <returns></returns>
        long CreateTown(long playerId, string townName, bool isCapital = false, ObjectPosition position = null);

        /// <summary>
        /// Get all towns of a certain player
        /// </summary>
        /// <param name="playerId">Id of the requested player</param>
        /// <returns>Enumeration of towns</returns>
        IEnumerable<Town> GetTownsOfPlayer(long playerId);

        /// <summary>
        /// Gets the town by specific town id
        /// </summary>
        /// <param name="townId">Id of the town</param>
        /// <returns>Retrieved town</returns>
        Town GetTown(long townId);
    }
}
