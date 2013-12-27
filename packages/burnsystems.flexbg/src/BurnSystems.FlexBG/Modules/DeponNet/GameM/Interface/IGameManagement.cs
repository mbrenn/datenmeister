using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.GameM.Interface
{
    /// <summary>
    /// Defines the game management
    /// </summary>
    public interface IGameManagement
    {
        /// <summary>
        /// Gets the games
        /// </summary>
        /// <returns>Enumeration of all games</returns>
        IEnumerable<Game> GetAll();

        /// <summary>
        /// Creates the game itself
        /// </summary>
        /// <param name="title">Title of the game</param>
        /// <param name="description">Description of the game</param>
        /// <param name="maxPlayers">Allowed players</param>
        /// <returns>Id of the created game</returns>
        long Create(string title, string description, int maxPlayers);

        /// <summary>
        /// Gets a game by id
        /// </summary>
        /// <param name="gameId">Id of the game</param>
        /// <returns>Game which has been found</returns>
        Game Get(long gameId);
    }
}
