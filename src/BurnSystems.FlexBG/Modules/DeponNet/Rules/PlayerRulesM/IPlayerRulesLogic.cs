using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.Rules.PlayerRulesM
{
    /// <summary>
    /// Defines the interface for the player rules
    /// </summary>
    public interface IPlayerRulesLogic
    {
        /// <summary>
        /// Creates a new player
        /// </summary>
        /// <param name="param">Parameters for the new player</param>
        /// <returns>Id of the new player</returns>
        long CreatePlayer(PlayerCreationParams param);

        /// <summary>
        /// Drops the player
        /// </summary>
        /// <param name="playerId">Id of the player to be dropped</param>
        void DropPlayer(long playerId);

        /// <summary>
        /// Gets a value whether the user may join the game
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="gameId">Id of the game</param>
        /// <returns>true, if user can join the game</returns>
        bool CanUserContinueGame(long userId, long gameId);
    }
}
