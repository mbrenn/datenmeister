using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.PlayerM.Interface
{
    public interface IPlayerManagement
    {
        /// <summary>
        /// Creates a new town into the database
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="gameId">Id of the game</param>
        /// <param name="playerName">Name of the player</param>
        /// <param name="empireName">Name of the empire</param>
        /// <returns>Id of the new player</returns>
        long CreatePlayer(long userId, long gameId, string playerName, string empireName);

        /// <summary>
        /// Removes the player
        /// </summary>
        /// <param name="playerId">Id of the player</param>
        void RemovePlayer(long playerId);

        /// <summary>
        /// Gets all players
        /// </summary>
        /// <returns>Enumeration of all players in all games</returns>
        IEnumerable<Player> GetAllPlayers();

        /// <summary>
        /// Gets the players of a specific user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>Players of the user</returns>
        IEnumerable<Player> GetPlayersOfUser(long userId);

        /// <summary>
        /// Gets the players of a specific game
        /// </summary>
        /// <param name="gameId">Id of the game</param>
        /// <returns>Players of the game</returns>
        IEnumerable<Player> GetPlayersOfGame(long gameId);

        /// <summary>
        /// Gets a specific player
        /// </summary>
        /// <param name="playerId">Id of the player</param>
        /// <returns>Player to be retrieved</returns>
        Player GetPlayer(long playerId);
    }
}
